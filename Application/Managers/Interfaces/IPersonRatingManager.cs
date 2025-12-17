using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IPersonRatingManager
{
    Task RateAsync(int userId, string nconst, int value, CancellationToken ct = default);
    Task<PageDto<PersonRatingDto>> GetRatingHistoryAsync(int userId, int page, int pageSize, CancellationToken ct = default);
}