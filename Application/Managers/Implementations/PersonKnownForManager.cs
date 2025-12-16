using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class PersonKnownForManager : IPersonKnownForManager
{
    private readonly IPersonKnownForRepository _repo;
    private readonly IMapper _mapper;
    public PersonKnownForManager(IPersonKnownForRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    /// <summary>
    /// Fetch person known for entries and attach HATEOAS links + pagination links (self/prev/next).
    /// </summary>
    public async Task<PageDto<PersonKnownForDto>> GetPersonKnownForAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetPersonKnownForAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/person-knownfor");
        foreach (var p in pageDto.Items)
        {
            p.Links.Add(new LinkDto("self", $"/api/person-knownfor/{p.Nconst}/{p.Tconst}"));
        }
        return pageDto;
    }

    public async Task<PersonKnownForDto?> GetByIdAsync(string nconst, string tconst, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(nconst, tconst, ct);
        if (entity == null) return null;
        var dto = _mapper.Map<PersonKnownForDto>(entity);
        dto.Links.Add(new LinkDto("self", $"/api/person-knownfor/{dto.Nconst}/{dto.Tconst}"));
        return dto;
    }

    public async Task<List<string>> GetMoviesByPersonAsync(string nconst, CancellationToken ct = default)
        => await _repo.GetMoviesByPersonAsync(nconst, ct);

    public async Task<List<string>> GetPeopleByMovieAsync(string tconst, CancellationToken ct = default)
        => await _repo.GetPeopleByMovieAsync(tconst, ct);

    /// <summary>
    /// Bygger pagination-links. Bruger '?' eller '&' alt efter om basePath har query i forvejen.
    /// </summary>
    private static void AddPageLinks<T>(PageDto<T> dto, string basePath)
    {
        var sep = basePath.Contains('?') ? '&' : '?';
        dto.Links.Add(new LinkDto("self", $"{basePath}{sep}page={dto.Page}&pageSize={dto.PageSize}"));
        if (dto.Page > 1)
            dto.Links.Add(new LinkDto("prev", $"{basePath}{sep}page={dto.Page - 1}&pageSize={dto.PageSize}"));
        var totalPages = (int)Math.Ceiling(dto.Total / (double)dto.PageSize);
        if (dto.Page < Math.Max(totalPages, 1))
            dto.Links.Add(new LinkDto("next", $"{basePath}{sep}page={dto.Page + 1}&pageSize={dto.PageSize}"));
    }
}