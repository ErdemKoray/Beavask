using Beavask.Application.DTOs.Role;
using Beavask.Application.Interface;
using Beavask.Application.Common;
using Beavask.Domain.Entities.Base;
using AutoMapper;
using Beavask.Application.Interface.Service;

namespace Beavask.Application.Service;

public class RoleService(IUnitOfWork unitOfWork, IMapper mapper) : IRoleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<RoleDto>> CreateAsync(RoleCreateDto roleCreateDto)
    {
        try
        {
            var role = _mapper.Map<Role>(roleCreateDto);
            await _unitOfWork.RoleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();

            var roleDto = _mapper.Map<RoleDto>(role);
            return Response<RoleDto>.Success(roleDto);
        }
        catch (Exception ex)
        {
            return Response<RoleDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null)
            {
                return Response<bool>.NotFound();
            }

            await _unitOfWork.RoleRepository.DeleteAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<RoleDto>>> GetAllAsync()
    {
        try
        {
            var roles = await _unitOfWork.RoleRepository.GetAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return Response<IEnumerable<RoleDto>>.Success(roleDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<RoleDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<RoleDto>> GetByIdAsync(int id)
    {
        try
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null)
            {
                return Response<RoleDto>.NotFound();
            }

            var roleDto = _mapper.Map<RoleDto>(role);
            return Response<RoleDto>.Success(roleDto);
        }
        catch (Exception ex)
        {
            return Response<RoleDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<RoleDto>> UpdateAsync(int id, RoleUpdateDto roleUpdateDto)
    {
        try
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null)
            {
                return Response<RoleDto>.NotFound();
            }

            _mapper.Map(roleUpdateDto, role);
            await _unitOfWork.RoleRepository.UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();

            var roleDto = _mapper.Map<RoleDto>(role);
            return Response<RoleDto>.Success(roleDto);
        }
        catch (Exception ex)
        {
            return Response<RoleDto>.Fail(ex.Message);
        }
    }
}

