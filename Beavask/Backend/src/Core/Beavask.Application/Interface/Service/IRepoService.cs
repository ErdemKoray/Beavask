// IRepoService.cs
using Beavask.Application.Common;
using Beavask.Application.DTOs;
using Beavask.Application.DTOs.Repo;


namespace Beavask.Application.Interface.Service
{
    public interface IRepoService
    {
        Task<Response<List<GitHubRepoDto>>> GetCurrentUserPublicRepositoriesAsync();
    }
}
