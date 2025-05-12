namespace Beavask.Application.DTOs.User;
public class UserBirefForCompany{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsRegistered { get; set; } = false;
    public bool IsAssignedToCompany { get; set; } = false;

}