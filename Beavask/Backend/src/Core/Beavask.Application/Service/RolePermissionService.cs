using Beavask.Application.DTOs.RolePermission;
using Beavask.Application.Interface.Service;
using Beavask.Application.Common;
using AutoMapper;
using Beavask.Domain.Entities.Join;
using Beavask.Application.Interface;

namespace Beavask.Application.Service;

public class RolePermissionService(IUnitOfWork unitOfWork, IMapper mapper) : IRolePermissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<RolePermissionDto>> CreateAsync(RolePermissionCreateDto rolePermissionCreateDto)
    {
        try
        {
            var rolePermission = _mapper.Map<RolePermission>(rolePermissionCreateDto);

            await _unitOfWork.RolePermissionRepository.AddAsync(rolePermission);
            
            await _unitOfWork.SaveChangesAsync();

            var rolePermissionDto = _mapper.Map<RolePermissionDto>(rolePermission);
            
            return Response<RolePermissionDto>.Success(rolePermissionDto);
        }
        catch (Exception ex)
        {
            return Response<RolePermissionDto>.Fail(ex.Message);
        }
    }


    public async Task<Response<RolePermissionDto>> GetByIdAsync(int id)
    {
        try
        {
            var rolePermission = await _unitOfWork.RolePermissionRepository.GetByIdAsync(id);
            if (rolePermission == null)
            {
                return Response<RolePermissionDto>.NotFound();
            }

            var rolePermissionDto = _mapper.Map<RolePermissionDto>(rolePermission);
            return Response<RolePermissionDto>.Success(rolePermissionDto);
        }
        catch (Exception ex)
        {
            return Response<RolePermissionDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<RolePermissionDto>>> GetAllAsync()
    {
        try
        {
            var rolePermissions = await _unitOfWork.RolePermissionRepository.GetAsync();
            var rolePermissionDtos = _mapper.Map<IEnumerable<RolePermissionDto>>(rolePermissions);
            return Response<IEnumerable<RolePermissionDto>>.Success(rolePermissionDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<RolePermissionDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var rolePermission = await _unitOfWork.RolePermissionRepository.GetByIdAsync(id);
            if (rolePermission == null)
            {
                return Response<bool>.NotFound();
            }

            await _unitOfWork.RolePermissionRepository.DeleteAsync(rolePermission);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
}

