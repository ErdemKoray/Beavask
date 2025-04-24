using Beavask.Application.DTOs.User;

namespace Beavask.Application.DTOs.Team
{
    public class TeamWithMembersDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public IEnumerable<UserDto> TeamMembers { get; set; } = new List<UserDto>();
    }
}