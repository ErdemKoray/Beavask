using Beavask.Application.Common;
using Beavask.Application.DTOs.Auth;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Helper;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Logging;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Service
{
    public class AuthService(IUnitOfWork unitOfWork, 
    ITokenGenerator tokenGenerator, IMailService mailService,
     ICurrentCompanyService currentCompanyService,
      ICurrentUserService currentUserService, ILogger logger) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly IMailService _mailService = mailService;
        private readonly ICurrentCompanyService _currentCompanyService = currentCompanyService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ILogger _logger = logger;

        public async Task<Response<bool>> RegisterAsync(RegisterRequestDto dto)
        {
            try
            {
                var emailExists = await _unitOfWork.UserRepository.ExistsAsync(u => u.Email == dto.Email);
                if (emailExists)
                {
                    await _logger.LogWarning("Registration attempt failed: Email already in use", context: dto.Email);
                    return Response<bool>.Fail("Email already in use.");
                }

                PasswordHelper.CreatePasswordHash(dto.Password, out string hash, out string salt);

                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.Username,
                    Email = dto.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                };
                var verificationCode = MailHelper.GenerateVerificationCode();
                if (verificationCode == null)
                {
                    await _logger.LogError("Registration failed: Verification code generation failed", context: dto.Email);
                    throw new ArgumentNullException(nameof(verificationCode), "Verification code cannot be null");
                }

                var verification = new VerificationCode
                {
                    Email = dto.Email,
                    Code = verificationCode,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.VerificationCodeRepository.AddAsync(verification);
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                await _mailService.SendIndividualVerificationCodeAsync(dto.Email, verificationCode);

                await _logger.LogInformation("User registration successful", context: dto.Email);
                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Registration failed", ex, context: dto.Email);
                return Response<bool>.Fail(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Response<string>> LoginAsync(LoginRequestDto dto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(u => u.Email == dto.Email);

                if (user == null)
                {
                    await _logger.LogWarning("Login attempt failed: User not found", context: dto.Email);
                    return Response<string>.Fail("User not found.");
                }

                if (!PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                {
                    await _logger.LogWarning("Login attempt failed: Invalid password", context: dto.Email, userId: user?.Id);
                    return Response<string>.Fail("Invalid password.");
                }

                var token = _tokenGenerator.GenerateToken(user);
                await _logger.LogInformation("User successfully logged in", context: user.Email, userId: user.Id);
                return Response<string>.Success(token);
            }
            catch (Exception ex)
            {
                var user = await _unitOfWork.UserRepository.GetSingleByConditionAsync(u => u.Email == dto.Email);
                if (user == null)
                    await _logger.LogError("Login failed: User not found", ex, context: dto.Email, userId: user?.Id);
                else
                    await _logger.LogError("Login failed", ex, context: dto.Email, userId: user.Id);
                return Response<string>.Fail("An error occurred during login.");
            }
        }
        public async Task<Response<string>> LoginCompanyAsync(CompanyLoginRequestDto dto)
        {
            try
            {
                var company = await _unitOfWork.CompanyRepository
                    .GetSingleByConditionAsync(c => c.Username == dto.Username && c.IsActive == true);

                if (company == null)
                {
                    await _logger.LogWarning("Company login attempt failed: Company not found", context: dto.Username, companyId: company?.Id);
                    return Response<string>.Fail("Company not found.");
                }

                if (!PasswordHelper.VerifyPassword(dto.Password, company.PasswordHash, company.PasswordSalt))
                {
                    await _logger.LogWarning("Company login attempt failed: Invalid password", context: dto.Username, companyId: company.Id);
                    return Response<string>.Fail("Invalid password.");
                }

                var token = _tokenGenerator.GenerateCompanyToken(company);
                await _logger.LogInformation("Company successfully logged in", context: company.Username, companyId: company.Id);
                return Response<string>.Success(token);
            }
            catch (Exception ex)
            {
                var company = await _unitOfWork.CompanyRepository.GetSingleByConditionAsync(c => c.Username == dto.Username);
                if (company == null)
                    await _logger.LogError("Company login failed: Company not found", ex, context: dto.Username, companyId: company?.Id);
                else
                    await _logger.LogError("Company login failed", ex, context: dto.Username, companyId: company.Id);
                return Response<string>.Fail($"An unexpected error occurred: {ex.Message}");
            }
        }
        

        public async Task<Response<string>> LoginWithGitHubAsync(GitHubLoginRequestDto dto, string clientId, string clientSecret)
        {
            try
            {
                var client = new HttpClient();

                var tokenRequest = new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "code", dto.Code }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token")
                {
                    Content = new FormUrlEncodedContent(tokenRequest)
                };
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    await _logger.LogError("GitHub access token request failed", context: dto.Code);
                    return Response<string>.Fail("GitHub access token alınamadı.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenJson = System.Text.Json.JsonDocument.Parse(content);
                var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

                if (string.IsNullOrEmpty(accessToken))
                {
                    await _logger.LogError("Empty GitHub access token received", context: dto.Code);
                    return Response<string>.Fail("Access token değeri boş.");
                }

                var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
                userRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                userRequest.Headers.UserAgent.ParseAdd("BeavaskApp");

                var userResponse = await client.SendAsync(userRequest);
                if (!userResponse.IsSuccessStatusCode)
                {
                    await _logger.LogError("GitHub user info request failed", context: dto.Code);
                    return Response<string>.Fail("GitHub kullanıcısı alınamadı.");
                }

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var githubUser = System.Text.Json.JsonSerializer.Deserialize<GitHubUserDto>(userJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                if (githubUser == null)
                {
                    await _logger.LogError("Failed to deserialize GitHub user data", context: dto.Code);
                    return Response<string>.Fail("GitHub kullanıcı bilgisi çözümlenemedi.");
                }

                var avatarUrl = githubUser.AvatarUrl ?? "https://default-avatar-url.com/default-avatar.png";
                
                var existingUser = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(u => u.UserName == githubUser.Login || u.Email == githubUser.Email);

                if (existingUser == null)
                {
                    var user = new User
                    {
                        UserName = githubUser.Login,
                        Email = githubUser.Email ?? $"{githubUser.Login}@github.local", 
                        AvatarUrl = avatarUrl,  
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        PasswordHash = "github", 
                        PasswordSalt = "github"  
                    };

                    await _unitOfWork.UserRepository.AddAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                    await _logger.LogInformation("New user created via GitHub login", context: user.Email, userId: user.Id);

                    existingUser = user;
                }

                var jwt = _tokenGenerator.GenerateToken(existingUser);
                await _logger.LogInformation("User successfully logged in via GitHub", context: existingUser.Email, userId: existingUser.Id);
                return Response<string>.Success(jwt);
            }
            catch (Exception ex)
            {
                await _logger.LogError("GitHub login failed", ex, context: dto.Code);
                return Response<string>.Fail($"Hata: {ex.Message}");
            }
        }
        public async Task<Response<bool>> RegisterCompanyAsync(CompanyCreateDto dto)
        {
            try
            {
                var company = new Company
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Website = dto.Website,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    AddressLine = dto.AddressLine,
                    City = dto.City,
                    Country = dto.Country,
                    PostalCode = dto.PostalCode,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = false
                };

                if (await _unitOfWork.CompanyRepository.GetSingleByConditionAsync(c => c.Email == dto.Email) != null)
                {
                    await _logger.LogWarning("Company registration attempted with existing email", context: dto.Email);
                    return Response<bool>.Fail("Email already exists.");
                }

                await _unitOfWork.CompanyRepository.AddAsync(company);
                await _unitOfWork.SaveChangesAsync();
                await _logger.LogInformation("New company registered", context: company.Email, companyId: company.Id);

                var verificationCode = MailHelper.GenerateVerificationCode();
                if (verificationCode == null)
                {
                    await _logger.LogError("Failed to generate verification code", context: company.Email, companyId: company.Id);
                    throw new ArgumentNullException(nameof(verificationCode), "Verification code cannot be null");
                }

                var verification = new VerificationCode
                {
                    Email = dto.Email,
                    Code = verificationCode,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.VerificationCodeRepository.AddAsync(verification);
                await _unitOfWork.SaveChangesAsync();
                await _mailService.SendVerificationCodeAsync(dto.Email, verificationCode);
                await _logger.LogInformation("Verification code sent to company", context: company.Email, companyId: company.Id);

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Company registration failed", ex, context: dto.Email);
                return Response<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Response<bool>> VerifyCompanyEmailAsync(string email, string code)
        {
            try
            {
                var verification = await _unitOfWork.VerificationCodeRepository
                    .GetSingleByConditionAsync(v => v.Email == email && v.Code == code && !v.IsUsed);
                if (verification == null){
                    var _company = await _unitOfWork.CompanyRepository.GetSingleByConditionAsync(c => c.Email == email);
                    await _unitOfWork.CompanyRepository.DeleteAsync(_company);
                    return Response<bool>.Fail("Out of time or invalid verification code.");
                }
                    
                verification.IsUsed = true;
                await _unitOfWork.VerificationCodeRepository.UpdateAsync(verification);
                await _unitOfWork.SaveChangesAsync();

                var company = await _unitOfWork.CompanyRepository.GetSingleByConditionAsync(c => c.Email == email);

                if (company == null)
                    return Response<bool>.Fail("Company not found.");
                var loginName = $"{company.Name.ToLower()}_admin";
                var password = PasswordHelper.GenerateRandomPassword();
                company.Username = loginName;
                PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt); 
                company.PasswordHash = passwordHash;
                company.PasswordSalt = passwordSalt;
                company.IsActive = true;
                await _unitOfWork.CompanyRepository.UpdateAsync(company);
                await _mailService.SendUserCredentialsAsync(company.Email, company.Username, password);

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail($"An eror occurred : {ex.Message}");
            }
        }

        public async Task<Response<bool>> VerifyPersonelEmailAsync(string email, string code)
        {
            try
            {
                var verification = await _unitOfWork.VerificationCodeRepository
                    .GetSingleByConditionAsync(v => v.Email == email && v.Code == code && !v.IsUsed);

                if (verification == null)
                {
                    var _user = await _unitOfWork.UserRepository.GetSingleByConditionAsync(u => u.Email == email);
                    await _unitOfWork.UserRepository.DeleteAsync(_user);
                    await _logger.LogWarning("Invalid verification code attempt", context: email);
                    return Response<bool>.Fail("Out of time or invalid verification code.");
                }

                verification.IsUsed = true;
                await _unitOfWork.VerificationCodeRepository.UpdateAsync(verification);
                await _unitOfWork.SaveChangesAsync();

                var user = await _unitOfWork.UserRepository.GetSingleByConditionAsync(u => u.Email == email);
                if (user == null)
                {
                    await _logger.LogError("User not found during email verification", context: email);
                    return Response<bool>.Fail("User not found.");
                }

                user.IsActive = true;
                await _unitOfWork.UserRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
                await _mailService.SendRegistrationSuccessEmailAsync(user.Email);
                await _logger.LogInformation("Personnel email verified successfully", context: email, userId: user.Id);
                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Personnel email verification failed", ex, context: email);
                return Response<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Response<bool>> ChangeCompanyPasswordAsync(ChangeCompanyPasswordRequestDto dto)
        {
            try
            {
                if (dto.NewPassword != dto.ConfirmNewPassword)
                {
                    await _logger.LogWarning("Password change failed - passwords do not match", 
                        context: $"CompanyId: {_currentCompanyService.CompanyId}");
                    return Response<bool>.Fail("New password and confirmation do not match.");
                }

                var company = await _unitOfWork.CompanyRepository
                    .GetSingleByConditionAsync(c => c.Id == _currentCompanyService.CompanyId && c.IsActive == true);
                if (company == null)
                {
                    await _logger.LogWarning("Password change failed - company not found", 
                        context: $"CompanyId: {_currentCompanyService.CompanyId}");
                    return Response<bool>.Fail("Company not found.");
                }

                if (!PasswordHelper.VerifyPassword(dto.OldPassword, company.PasswordHash, company.PasswordSalt))
                {
                    await _logger.LogWarning("Password change failed - invalid old password", 
                        context: $"CompanyId: {_currentCompanyService.CompanyId}");
                    return Response<bool>.Fail("Invalid old password.");
                }

                PasswordHelper.CreatePasswordHash(dto.NewPassword, out string newHash, out string newSalt);
                company.PasswordHash = newHash;
                company.PasswordSalt = newSalt;

                await _unitOfWork.CompanyRepository.UpdateAsync(company);
                await _unitOfWork.SaveChangesAsync();

                await _logger.LogInformation("Company password changed successfully", 
                    context: $"CompanyId: {_currentCompanyService.CompanyId}", 
                    companyId: company.Id);
                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await _logger.LogError("Password change failed", ex, 
                    context: $"CompanyId: {_currentCompanyService.CompanyId}");
                return Response<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }
        public async System.Threading.Tasks.Task AcceptInvitationAsync(string token, int userId)
        {
            var invitation = await _unitOfWork.InvitationTokenRepository.GetByTokenAsync(token);

            if (invitation == null)
                throw new Exception("Invitation not found.");
            if (invitation.IsUsed)
                throw new Exception("Invitation already used.");
            if (invitation.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invitation expired.");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId) ?? throw new Exception("User not found.");
            user.CompanyId = invitation.CompanyId;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            invitation.IsUsed = true;
            await _unitOfWork.InvitationTokenRepository.UpdateAsync(invitation);

            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(invitation.ProjectId);
            await _unitOfWork.ProjectMemberRepository.AddAsync(new ProjectMember
            {
                UserId = user.Id,
                ProjectId = invitation.ProjectId,
                IsActive = true,
                User = user,
                Project = project
            });
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
