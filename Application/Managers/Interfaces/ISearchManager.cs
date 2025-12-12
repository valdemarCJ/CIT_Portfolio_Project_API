using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface ISearchManager
{
    Task<PageDto<SearchDto>> StringSearchAsync(int userId, string text, int page, int pageSize, CancellationToken ct = default);
    Task<PageDto<SearchDto>> StructuredSearchAsync(int userId, string? title, string? plot, string? characters, string? person, int page, int pageSize, CancellationToken ct = default);
    Task<PageDto<SearchHistoryDto>> GetSearchHistoryAsync(int userId, int page, int pageSize, CancellationToken ct = default);
}
