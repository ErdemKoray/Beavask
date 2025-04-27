using System.Text.Json.Serialization;

namespace Beavask.Application.DTOs.Repo;
public class GitHubRepoDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}