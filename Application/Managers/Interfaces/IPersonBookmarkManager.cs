using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IPersonBookmarkManager
{
    Task AddAsync(int userId, string nconst, string? note, CancellationToken ct = default);
    Task<PageDto<PersonBookmarkDto>> GetAsync(int userId, int page, int pageSize, CancellationToken ct = default);
    Task DeleteAsync(int userId, string nconst, CancellationToken ct = default);
}