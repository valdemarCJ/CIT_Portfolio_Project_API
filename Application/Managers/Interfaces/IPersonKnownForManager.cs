using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IPersonKnownForManager
{
    Task<PageDto<PersonKnownForDto>> GetPersonKnownForAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PersonKnownForDto?> GetByIdAsync(string nconst, string tconst, CancellationToken ct = default);
    Task<List<string>> GetMoviesByPersonAsync(string nconst, CancellationToken ct = default);
    Task<List<string>> GetPeopleByMovieAsync(string tconst, CancellationToken ct = default);
}