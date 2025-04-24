using Beavask.Application.Common;
using Beavask.Application.DTOs.Auth;
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

        public AuthService(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
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
    }
}
