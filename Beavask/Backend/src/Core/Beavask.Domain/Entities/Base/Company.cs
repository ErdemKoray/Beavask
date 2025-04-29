namespace Beavask.Domain.Entities.Base
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Contact Information
        public string Website { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;

        // Address Information
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        // Company User and Auth Information
        public string Username { get; set; } = string.Empty; // Company Login Username
        public string PasswordHash { get; set; } = string.Empty; // Hashed Password

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property for associated users
        public ICollection<User> Users = new List<User>();
    }
}
