using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IMovieDetailManager
{
    Task<PageDto<MovieDetailDto>> GetMovieDetailsAsync(int page, int pageSize, CancellationToken ct = default);
    Task<MovieDetailDto?> GetByIdAsync(string tconst, CancellationToken ct = default);
}