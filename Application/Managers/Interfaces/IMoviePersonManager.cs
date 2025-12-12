using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IMoviePersonManager
{
    Task<PageDto<MoviePersonDto>> GetMoviePeopleAsync(int page, int pageSize, CancellationToken ct = default);
    Task<List<MoviePersonDto>> GetPeopleByMovieAsync(string tconst, CancellationToken ct = default);
}