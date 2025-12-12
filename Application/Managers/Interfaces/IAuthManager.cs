using CIT_Portfolio_Project_API.Application.DTOs.Auth;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IAuthManager
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
