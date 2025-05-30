using Beavask.Application.Common;
using Beavask.Application.DTOs.Company;
using Beavask.Application.DTOs.Project;
using Beavask.Application.DTOs.User;

namespace Beavask.Application.Interface.Service;

public interface ICompanyService
{
    Task<Response<CompanyDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<CompanyDto>>> GetAllAsync();
    Task<Response<CompanyDto>> UpdateAsync(int id, CompanyUpdateDto companyUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
    Task<Response<IEnumerable<UserBirefForCompany>>> GetAllUsersByCompanyIdAsync(int companyId);
    Task<Response<IEnumerable<ProjectDto>>> GetAllProjectsByCompanyIdAsync();
    Task<Response<IEnumerable<UserDto>>> GetAllUsersByCompanyProjectIdAsync(int projectId);
    Task<Response<IEnumerable<UserBirefForCompany>>> GetAllUsersAccountDetailsByCompanyProjectIdAsync(int projectId);
}

