namespace Beavask.Application.Helper
{
    public static class UrlHelper
    {
        public static string ConvertToGitHubApiUrl(string repoUrl)
        {
            if (string.IsNullOrWhiteSpace(repoUrl))
                throw new ArgumentException("Repository URL cannot be null or empty.", nameof(repoUrl));

            var uri = new Uri(repoUrl);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length >= 2)
            {
                var user = segments[0];
                var repo = segments[1];
                return $"https://api.github.com/repos/{user}/{repo}";
            }

            throw new ArgumentException("Invalid GitHub repository URL format.");
        }
    }
}
