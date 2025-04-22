using Beavask.Application.DTOs.Permission;
using Beavask.Application.Interfaces;
using AutoMapper;
using Beavask.Domain.Entities.Base;
using Beavask.Application.Common;
using Beavask.Application.Interface.Service;

namespace Beavask.Application.Service;

public class PermissionService(IUnitOfWork unitOfWork, IMapper mapper) : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<PermissionDto>> CreateAsync(PermissionCreateDto permissionCreateDto)
    {
        try
        {
            var permission = _mapper.Map<Permission>(permissionCreateDto);
            await _unitOfWork.PermissionRepository.AddAsync(permission);
            await _unitOfWork.SaveChangesAsync();
            var permissionDto = _mapper.Map<PermissionDto>(permission);
            return Response<PermissionDto>.Success(permissionDto);
        }
        catch (Exception ex)
        {
            return Response<PermissionDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<PermissionDto>> UpdateAsync(int id, PermissionUpdateDto permissionUpdateDto)
    {
        try
        {
            var permission = await _unitOfWork.PermissionRepository.GetByIdAsync(id);
            if (permission == null)
                return Response<PermissionDto>.NotFound();

            _mapper.Map(permissionUpdateDto, permission);
            await _unitOfWork.PermissionRepository.UpdateAsync(permission);
            await _unitOfWork.SaveChangesAsync();
            var permissionDto = _mapper.Map<PermissionDto>(permission);
            return Response<PermissionDto>.Success(permissionDto);
        }
        catch (Exception ex)
        {
            return Response<PermissionDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var permission = await _unitOfWork.PermissionRepository.GetByIdAsync(id);
            if (permission == null)
                return Response<bool>.NotFound();

            await _unitOfWork.PermissionRepository.DeleteAsync(permission);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<PermissionDto>> GetByIdAsync(int id)
    {
        try
        {
            var permission = await _unitOfWork.PermissionRepository.GetByIdAsync(id);
            if (permission == null)
                return Response<PermissionDto>.NotFound();

            var permissionDto = _mapper.Map<PermissionDto>(permission);
            return Response<PermissionDto>.Success(permissionDto);
        }
        catch (Exception ex)
        {
            return Response<PermissionDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<PermissionDto>>> GetAllAsync()
    {
        try
        {
            var permissions = await _unitOfWork.PermissionRepository.GetAsync();
            var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);
            return Response<IEnumerable<PermissionDto>>.Success(permissionDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<PermissionDto>>.Fail(ex.Message);
        }
    }
}

