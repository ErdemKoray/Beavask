using Beavask.Application.Common;
using Beavask.Application.DTOs.Auth;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Helper;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;
using System.Security.Cryptography;
using System.Text;

namespace Beavask.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMailService _mailService;


        public AuthService(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
            _mailService = mailService;
        }

        public async Task<Response<bool>> RegisterAsync(RegisterRequestDto dto)
        {
            try
            {
                var emailExists = await _unitOfWork.UserRepository.ExistsAsync(u => u.Email == dto.Email);
                if (emailExists)
                    return Response<bool>.Fail("Email already in use.");

                CreatePasswordHash(dto.Password, out string hash, out string salt);

                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.Username,
                    Email = dto.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Response<string>> LoginAsync(LoginRequestDto dto)
        {
            var user = await _unitOfWork.UserRepository
                .GetSingleByConditionAsync(u => u.Email == dto.Email);

            if (user == null)
                return Response<string>.Fail("User not found.");

            if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                return Response<string>.Fail("Invalid password.");

            var token = _tokenGenerator.GenerateToken(user);
            return Response<string>.Success(token);
        }

        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new HMACSHA256();
            salt = Convert.ToBase64String(hmac.Key);
            hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string password, string hash, string salt)
        {
            using var hmac = new HMACSHA256(Convert.FromBase64String(salt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == hash;
        }
        public async Task<Response<string>> LoginWithGitHubAsync(GitHubLoginRequestDto dto, string clientId, string clientSecret)
        {
            try
            {
                var client = new HttpClient();

                // 1. GitHub Access Token Al
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
                    return Response<string>.Fail("GitHub access token alınamadı.");

                var content = await response.Content.ReadAsStringAsync();
                var tokenJson = System.Text.Json.JsonDocument.Parse(content);
                var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

                if (string.IsNullOrEmpty(accessToken))
                    return Response<string>.Fail("Access token değeri boş.");

                // 2. GitHub User Getir
                var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
                userRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                userRequest.Headers.UserAgent.ParseAdd("BeavaskApp");

                var userResponse = await client.SendAsync(userRequest);
                if (!userResponse.IsSuccessStatusCode)
                    return Response<string>.Fail("GitHub kullanıcısı alınamadı.");

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var githubUser = System.Text.Json.JsonSerializer.Deserialize<GitHubUserDto>(userJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Console.WriteLine($"Avatar URL: {githubUser.AvatarUrl}");
                
                if (githubUser == null)
                    return Response<string>.Fail("GitHub kullanıcı bilgisi çözümlenemedi.");

                // Avatar URL kontrolü ve loglama
                var avatarUrl = githubUser.AvatarUrl ?? "https://default-avatar-url.com/default-avatar.png"; // Fallback URL
                Console.WriteLine("Avatar URL: " + avatarUrl);  // Debug: Avatar URL'yi logla
                
                // 3. Kullanıcı DB'de var mı kontrol et
                var existingUser = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(u => u.UserName == githubUser.Login || u.Email == githubUser.Email);

                if (existingUser == null)
                {
                    // Yeni kullanıcı oluştur
                    var user = new User
                    {
                        UserName = githubUser.Login,
                        Email = githubUser.Email ?? $"{githubUser.Login}@github.local", // Eğer e-posta boşsa fallback kullan
                        AvatarUrl = avatarUrl,  // Avatar URL'sini kaydediyoruz
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        PasswordHash = "github", // GitHub kullanıcı kaydı için geçici değer
                        PasswordSalt = "github"  // GitHub kullanıcı kaydı için geçici değer
                    };

                    await _unitOfWork.UserRepository.AddAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    existingUser = user;
                }

                // JWT Token üret
                var jwt = _tokenGenerator.GenerateToken(existingUser);
                return Response<string>.Success(jwt);
            }
            catch (Exception ex)
            {
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
                return Response<bool>.Fail("Email already exists.");

            var loginName = $"{dto.Name.ToLower()}_admin";
            var password = PasswordHelper.GenerateRandomPassword();

            company.Username = loginName;
            PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt); 
            company.PasswordHash = passwordHash;
            company.PasswordSalt = passwordSalt;

            await _unitOfWork.CompanyRepository.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();

            var verificationCode = MailHelper.GenerateVerificationCode();
            if (verificationCode == null)
                throw new ArgumentNullException(nameof(verificationCode), "Verification code cannot be null");
            else
            {
                Console.WriteLine("verificataion code ",verificationCode);
            }
            var verification = new VerificationCode
            {
                Email = dto.Email,
                Code = verificationCode,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.VerificationCodeRepository.AddAsync(verification);
            await _unitOfWork.SaveChangesAsync();
            Console.WriteLine($"Email: {dto.Email}");
            Console.WriteLine($"Verification Code: {verificationCode}");
            await _mailService.SendVerificationCodeAsync(dto.Email, verificationCode);

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
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
            company.IsActive = true;
            await _unitOfWork.CompanyRepository.UpdateAsync(company);
            var randomPassword = PasswordHelper.GenerateRandomPassword();
            await _mailService.SendUserCredentialsAsync(company.Email, company.Username, randomPassword);

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail($"An eror occurred : {ex.Message}");
        }
    }

    }
}
