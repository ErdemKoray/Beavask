using System.Text.Json.Serialization;

namespace Beavask.Application.DTOs.Repo;
public class GitHubContributorDto
{
    [JsonPropertyName("login")]
    public string Username { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("contributions")]
    public int Contributions { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    public bool IsRegistered { get; set; }
}
