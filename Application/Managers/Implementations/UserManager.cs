using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class UserManager : IUserManager
{
    private readonly IUserRepository _repo;
    private readonly PasswordHasher _hasher;
    private readonly IMapper _mapper;
    public UserManager(IUserRepository repo, PasswordHasher hasher, IMapper mapper)
    { _repo = repo; _hasher = hasher; _mapper = mapper; }

    /// <summary>
    /// Registers a new user: checks for duplicate username, hashes the password, and persists.
    /// Security baseline: never store plaintext passwords.
    /// </summary>
    public async Task<UserDto> RegisterAsync(string username, string email, string password, CancellationToken ct = default)
    {
        var existing = await _repo.GetByUsernameAsync(username, ct);
        if (existing != null) throw new InvalidOperationException("Username already exists");
        var user = new User { Username = username, Email = email, PasswordHash = _hasher.Hash(password) };
        user = await _repo.AddAsync(user, ct);
        var dto = _mapper.Map<UserDto>(user);
        dto.Links.Add(new DTOs.LinkDto("self", $"/api/users/{dto.Id}"));
        return dto;
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user == null) return null;
        var dto = _mapper.Map<UserDto>(user);
        dto.Links.Add(new DTOs.LinkDto("self", $"/api/users/{dto.Id}"));
        return dto;
    }
}
