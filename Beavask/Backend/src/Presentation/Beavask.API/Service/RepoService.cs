using Beavask.Application.Common;
using Beavask.Application.DTOs;
using Beavask.Application.DTOs.Repo;
using Beavask.Application.Helper;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Service;
using System.Text.Json;

namespace Beavask.API.Service
{
    public class RepoService(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRepoService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<Response<List<GitHubRepoDto>>> GetCurrentUserPublicRepositoriesAsync()
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (userId == null)
                    return Response<List<GitHubRepoDto>>.Fail("Kullanıcı kimliği bulunamadı.");

                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId.Value);
                if (user == null || string.IsNullOrEmpty(user.UserName))
                    return Response<List<GitHubRepoDto>>.Fail("Kullanıcı veya GitHub kullanıcı adı bulunamadı.");

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("BeavaskApp");

                var response = await client.GetAsync($"https://api.github.com/users/{user.UserName}/repos");

                if (!response.IsSuccessStatusCode)
                    return Response<List<GitHubRepoDto>>.Fail("GitHub repository bilgileri alınamadı.");

                var content = await response.Content.ReadAsStringAsync();

                var repositories = JsonSerializer.Deserialize<List<GitHubRepoDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Response<List<GitHubRepoDto>>.Success(repositories ?? new List<GitHubRepoDto>());
            }
            catch (Exception ex)
            {
                return Response<List<GitHubRepoDto>>.Fail($"Hata: {ex.Message}");
            }
        }

        public async Task<Response<GitHubRepoDto>> GetSingleRepositoryDetailAsync(string repoApiUrl)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("BeavaskApp");
                var url = UrlHelper.ConvertToGitHubApiUrl(repoApiUrl);
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return Response<GitHubRepoDto>.Fail("Could not get the repository details.");

                var content = await response.Content.ReadAsStringAsync();
                var repository = JsonSerializer.Deserialize<GitHubRepoDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Response<GitHubRepoDto>.Success(repository);
            }
            catch (Exception ex)
            {
                return Response<GitHubRepoDto>.Fail($"Error: {ex.Message}");
            }
        }
        public async Task<Response<List<GitHubContributorDto>>> GetRepositoryContributorsAsync(string repoWebUrl)
        {
            var apiBaseUrl = UrlHelper.ConvertToGitHubApiUrl(repoWebUrl);
            var contributorsApiUrl = $"{apiBaseUrl}/contributors";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("BeavaskApp");

            var response = await client.GetAsync(contributorsApiUrl);
            if (!response.IsSuccessStatusCode)
                return Response<List<GitHubContributorDto>>.Fail("Could not fetch contributors.");

            var content = await response.Content.ReadAsStringAsync();
            var contributors = JsonSerializer.Deserialize<List<GitHubContributorDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<GitHubContributorDto>();

            // 1. GitHub login'larını topla
            var loginList = contributors.Select(c => c.Username).Distinct().ToList();

            // 2. Veritabanında bu login'lere sahip kullanıcıları çek
            var registeredUsers = await _unitOfWork.UserRepository
                .GetWhereAsync(u => loginList.Contains(u.UserName));

            var registeredUsernames = new HashSet<string>(registeredUsers.Select(u => u.UserName));

            // 3. Her contributor'a kayıt durumunu ata
            foreach (var contributor in contributors)
            {
                contributor.IsRegistered = registeredUsernames.Contains(contributor.Username);
            }

            return Response<List<GitHubContributorDto>>.Success(contributors);
        }
    }
}

