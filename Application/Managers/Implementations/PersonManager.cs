using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class PersonManager : IPersonManager
{
    private readonly IPersonRepository _repo;
    private readonly IMapper _mapper;
    public PersonManager(IPersonRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    /// <summary>
    /// Returns a paged list of people with HATEOAS links (self/prev/next and per-item self).
    /// </summary>
    public async Task<PageDto<PersonDto>> GetPeopleAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetPeopleAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/people");
        foreach (var p in pageDto.Items)
            p.Links.Add(new LinkDto("self", $"/api/people/{p.Nconst}"));
        return pageDto;
    }

    /// <summary>
    /// Looks up a person by nconst and adds a self link if found; returns null if missing.
    /// </summary>
    public async Task<PersonDto?> GetByIdAsync(string nconst, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(nconst, ct);
        if (entity == null) return null;
        var dto = _mapper.Map<PersonDto>(entity);
        dto.Links.Add(new LinkDto("self", $"/api/people/{dto.Nconst}"));
        return dto;
    }

    /// <summary>
    /// Adds self/prev/next paging links for consistency across list endpoints.
    /// </summary>
    private static void AddPageLinks<T>(PageDto<T> dto, string basePath)
    {
        dto.Links.Add(new LinkDto("self", $"{basePath}?page={dto.Page}&pageSize={dto.PageSize}"));
        if (dto.Page > 1)
            dto.Links.Add(new LinkDto("prev", $"{basePath}?page={dto.Page - 1}&pageSize={dto.PageSize}"));
        var totalPages = (int)Math.Ceiling(dto.Total / (double)dto.PageSize);
        if (dto.Page < Math.Max(totalPages, 1))
            dto.Links.Add(new LinkDto("next", $"{basePath}?page={dto.Page + 1}&pageSize={dto.PageSize}"));
    }
}
