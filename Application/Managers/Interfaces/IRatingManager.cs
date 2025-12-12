using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IRatingManager
{
    Task RateAsync(int userId, string tconst, int value, CancellationToken ct = default);
    Task<PageDto<RatingHistoryDto>> GetRatingHistoryAsync(int userId, int page, int pageSize, CancellationToken ct = default);
}
