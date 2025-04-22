using Beavask.Application.Common;
using Beavask.Application.DTOs.UserRole;

namespace Beavask.Application.Interface.Service;

public interface IUserRoleService
{
    Task<Response<UserRoleDto>> CreateAsync(UserRoleCreateDto userRoleCreateDto);
    Task<Response<IEnumerable<UserRoleDto>>> GetAllAsync();
}

