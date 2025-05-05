using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Service
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
        string GenerateCompanyToken(Company company);
    }
}
