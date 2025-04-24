namespace Beavask.Application.Interface.Service;
public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    string? FirstName { get; }
    string? LastName { get; }
}

