using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.UserRole;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interfaces;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Service;

public class UserRoleService(IUnitOfWork unitOfWork, IMapper mapper) : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<UserRoleDto>> CreateAsync(UserRoleCreateDto userRoleCreateDto)
    {
        try
        {
            var userRole = _mapper.Map<UserRole>(userRoleCreateDto);
            await _unitOfWork.UserRoleRepository.AddAsync(userRole);
            await _unitOfWork.SaveChangesAsync();
            var userRoleDto = _mapper.Map<UserRoleDto>(userRole);
            return Response<UserRoleDto>.Success(userRoleDto);
        }
        catch (Exception ex)
        {
            return Response<UserRoleDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<UserRoleDto>>> GetAllAsync()
    {
        try
        {
            var userRoles = await _unitOfWork.UserRoleRepository.GetAsync();
            var userRoleDtos = _mapper.Map<IEnumerable<UserRoleDto>>(userRoles);
            return Response<IEnumerable<UserRoleDto>>.Success(userRoleDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<UserRoleDto>>.Fail(ex.Message);
        }
    }
}

