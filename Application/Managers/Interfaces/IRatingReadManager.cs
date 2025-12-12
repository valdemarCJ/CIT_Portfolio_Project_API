using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IRatingReadManager
{
    Task<PageDto<ImdbRatingDto>> GetRatingsAsync(int page, int pageSize, CancellationToken ct = default);
    Task<ImdbRatingDto?> GetByIdAsync(string tconst, CancellationToken ct = default);
}