namespace Beavask.Application.DTOs.Auth
{
    public class GitHubUserDto
    {
        public string Login { get; set; } = string.Empty;
        public string? Email { get; set; }
        
        [System.Text.Json.Serialization.JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
    }
}
