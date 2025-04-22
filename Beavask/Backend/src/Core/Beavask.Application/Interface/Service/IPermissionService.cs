using Beavask.Application.DTOs.Permission;
using Beavask.Application.Common;

namespace Beavask.Application.Interface.Service;

public interface IPermissionService
{
    Task<Response<PermissionDto>> CreateAsync(PermissionCreateDto permissionCreateDto);
    Task<Response<PermissionDto>> UpdateAsync(int id, PermissionUpdateDto permissionUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
    Task<Response<PermissionDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<PermissionDto>>> GetAllAsync();
}

