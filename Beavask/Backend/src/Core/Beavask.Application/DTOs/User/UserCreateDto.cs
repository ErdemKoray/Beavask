namespace Beavask.Application.DTOs.User
{
    public class UserCreateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public int CompanyId { get; set; }
    }
}
