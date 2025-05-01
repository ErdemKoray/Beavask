namespace Beavask.Application.DTOs.User;
public class UserWithTeamAndCompanyDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int? TeamId { get; set; }
    public int? CompanyId { get; set; }

    public string? TeamName { get; set; }
    public string? CompanyName { get; set; }
}
