using Beavask.Application.Common;
using Beavask.Application.DTOs.Customer;

namespace Beavask.Application.Interface.Service;
public interface ICustomerService
{
    Task<Response<CustomerDto>> CreateAsync(CustomerCreateDto customerCreateDto);
    Task<Response<CustomerDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<CustomerDto>>> GetAllAsync();
    Task<Response<CustomerDto>> UpdateAsync(int id, CustomerUpdateDto customerUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
}
