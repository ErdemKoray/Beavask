using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ICustomerRepository : IBaseRepository<Customer, int>
{
    // Add any customer-specific repository methods here
} 