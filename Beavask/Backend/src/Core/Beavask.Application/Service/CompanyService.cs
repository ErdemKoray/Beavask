using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;
using Beavask.Application.Helper;

namespace Beavask.Application.Service;

public class CompanyService(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService) : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IMailService _mailService = mailService;

    public async Task<Response<CompanyDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<CompanyDto>.NotFound();

            var dto = _mapper.Map<CompanyDto>(entity);
            return Response<CompanyDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<CompanyDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<CompanyDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _unitOfWork.CompanyRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Response<IEnumerable<CompanyDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<CompanyDto>>.Fail(ex.Message);
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

    public async Task<Response<bool>> VerifyEmailAsync(string email, string code)
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

    public async Task<Response<CompanyDto>> UpdateAsync(int id, CompanyUpdateDto companyUpdateDto)
    {
        try
        {
            var existingEntity = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
            if (existingEntity == null)
                return Response<CompanyDto>.NotFound();

            _mapper.Map(companyUpdateDto, existingEntity);
            await _unitOfWork.CompanyRepository.UpdateAsync(existingEntity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CompanyDto>(existingEntity);
            return Response<CompanyDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<CompanyDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<bool>.NotFound();

            await _unitOfWork.CompanyRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
}

