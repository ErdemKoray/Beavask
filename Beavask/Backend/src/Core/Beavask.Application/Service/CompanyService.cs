using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;

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
            // Şirket kaydını yap
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
                IsActive = true
            };

            // Şirketin login ismini ve şifresini oluştur
            var loginName = $"{dto.Name.ToLower()}_admin";
            var password = GenerateRandomPassword();

            company.Username = loginName;
            company.PasswordHash = password; // Şifre hash'leme yapılmalı

            await _unitOfWork.CompanyRepository.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();

            // Kullanıcıya mail gönder
            await _mailService.SendUserCredentialsAsync(company.Email, loginName, password);

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail($"Şirket kaydederken hata oluştu: {ex.Message}");
        }
    }

    private string GenerateRandomPassword()
    {
        // Rastgele bir şifre oluştur (en az 8 karakter olmalı)
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
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

