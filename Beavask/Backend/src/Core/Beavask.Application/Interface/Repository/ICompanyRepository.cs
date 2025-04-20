using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ICompanyRepository : IBaseRepository<Company, int>
{
    // Add any company-specific repository methods here
} 