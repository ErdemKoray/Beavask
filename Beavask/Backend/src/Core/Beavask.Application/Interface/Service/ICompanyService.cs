using Beavask.Application.Common;
using Beavask.Application.DTOs.Company;

namespace Beavask.Application.Interface.Service;

public interface ICompanyService
{
    Task<Response<CompanyDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<CompanyDto>>> GetAllAsync();
    Task<Response<CompanyDto>> CreateAsync(CompanyCreateDto companyCreateDto);
    Task<Response<CompanyDto>> UpdateAsync(int id, CompanyUpdateDto companyUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
}

