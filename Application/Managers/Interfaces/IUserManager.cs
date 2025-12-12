using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IUserManager
{
    Task<UserDto> RegisterAsync(string username, string email, string password, CancellationToken ct = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default);
}
