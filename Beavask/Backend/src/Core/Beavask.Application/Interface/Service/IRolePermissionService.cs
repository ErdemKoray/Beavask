using Beavask.Application.DTOs.RolePermission;
using Beavask.Application.Common;

namespace Beavask.Application.Interface.Service
{
    public interface IRolePermissionService
    {
        Task<Response<RolePermissionDto>> CreateAsync(RolePermissionCreateDto rolePermissionCreateDto);
        Task<Response<RolePermissionDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<RolePermissionDto>>> GetAllAsync();
        Task<Response<bool>> DeleteAsync(int id);
    }
}
