using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IWordIndexManager
{
    Task<PageDto<WordIndexDto>> GetWordIndexAsync(int page, int pageSize, CancellationToken ct = default);
    Task<List<WordIndexDto>> GetWordsByMovieAsync(string tconst, CancellationToken ct = default);
}