using Beavask.Application.DTOs.Role;
using Beavask.Application.Common;

namespace Beavask.Application.Interface.Service
{
    public interface IRoleService
    {
        Task<Response<RoleDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<RoleDto>>> GetAllAsync();
        Task<Response<RoleDto>> CreateAsync(RoleCreateDto roleCreateDto);
        Task<Response<RoleDto>> UpdateAsync(int id, RoleUpdateDto roleUpdateDto);
        Task<Response<bool>> DeleteAsync(int id);
        Task<Response<RoleDto>> AssignRoleToUserAsync(int userId, int roleId);
    }
}
