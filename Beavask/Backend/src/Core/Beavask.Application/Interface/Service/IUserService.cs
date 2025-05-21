using Beavask.Application.Common;
using Beavask.Application.DTOs.User;

namespace Beavask.Application.Interface.Service;

public interface IUserService
{
    Task<Response<UserWithTeamAndCompanyDto>> GetUserBriefAsync(int id);
    Task<Response<IEnumerable<UserDto>>> GetAllAsync();
    Task<Response<UserDto>> CreateAsync(UserCreateDto userCreateDto);
    Task<Response<UserDto>> UpdateAsync(int id, UserUpdateDto userUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
    Task<Response<bool>> UpdateUserCompanyIdAsync(int id, UpdateCompanyRequest updateCompanyRequest);
}

