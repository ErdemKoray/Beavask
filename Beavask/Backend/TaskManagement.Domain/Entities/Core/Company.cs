using System;
using System.Collections.Generic;

namespace TaskManagement.Domain.Entities.Core
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }  = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Conntact
        public string Website { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;

        // Adresses
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // User - Company one-to-many relationship
        public ICollection<User> Users = new List<User>();
    }
}