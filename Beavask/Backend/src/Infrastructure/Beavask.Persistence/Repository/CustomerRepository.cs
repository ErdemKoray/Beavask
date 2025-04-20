using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class CustomerRepository : BaseRepository<Customer, int>, ICustomerRepository
{
    public CustomerRepository(DbContext context) : base(context)
    {
    }
} 