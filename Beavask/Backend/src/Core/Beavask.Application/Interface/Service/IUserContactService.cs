using Beavask.Application.Common;
using Beavask.Application.DTOs.UserContact;

namespace Beavask.Application.Interface.Service;
public interface IUserContactService
{
    Task<Response<UserContactDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<UserContactDto>>> GetAllAsync();
    Task<Response<IEnumerable<UserContactDto>>> GetByUserIdAsync(int userId);
    Task<Response<UserContactDto>> CreateAsync(UserContactCreateDto userContactCreate);
    Task<Response<UserContactDto>> UpdateAsync(int id, UserContactCreateDto userContactCreate);
    Task<Response<bool>> DeleteAsync(int id);
}
