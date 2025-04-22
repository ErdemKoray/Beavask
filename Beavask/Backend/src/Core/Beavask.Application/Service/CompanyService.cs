using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;

public class CompanyService(IUnitOfWork unitOfWork, IMapper mapper) : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

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

    public async Task<Response<CompanyDto>> CreateAsync(CompanyCreateDto companyCreateDto)
    {
        try
        {
            var entity = _mapper.Map<Company>(companyCreateDto);
            await _unitOfWork.CompanyRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CompanyDto>(entity);
            return Response<CompanyDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<CompanyDto>.Fail(ex.Message);
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

