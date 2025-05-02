namespace Beavask.Application.Interface.Service;
public interface ICurrentCompanyService
{
    int? CompanyId { get; }
    string? CompanyName { get; }
    string? CompanyEmail { get; }
    string? CompanyPhone { get; }
    string? CompanyLogoUrl { get; }
    string? CompanyWebsite { get; }
    string? CompanyDescription { get; }

}