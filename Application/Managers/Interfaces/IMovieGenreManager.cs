using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IMovieGenreManager
{
    Task<PageDto<MovieGenreDto>> GetMovieGenresAsync(int page, int pageSize, CancellationToken ct = default);
    Task<List<MovieGenreDto>> GetGenresByMovieAsync(string tconst, CancellationToken ct = default);
}