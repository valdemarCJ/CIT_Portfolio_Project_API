using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class MovieManager : IMovieManager
{
    private readonly IMovieRepository _repo;
    private readonly IMapper _mapper;
    public MovieManager(IMovieRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    /// <summary>
    /// Fetch movies and attach HATEOAS links + pagination links (self/prev/next).
    /// </summary>
    public async Task<PageDto<MovieDto>> GetMoviesAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetMoviesAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/movies");
        foreach (var m in pageDto.Items)
        {
            m.Links.Add(new LinkDto("self", $"/api/movies/{m.Tconst}"));
        }
        return pageDto;
    }

    public async Task<MovieDto?> GetByIdAsync(string tconst, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(tconst, ct);
        if (entity == null) return null;
        var dto = _mapper.Map<MovieDto>(entity);
        dto.Links.Add(new LinkDto("self", $"/api/movies/{dto.Tconst}"));
        return dto;
    }

    public async Task<PageDto<MovieDto>> GetMovieTypesAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetMovieTypesAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/movies/types/movies");
        foreach (var m in pageDto.Items)
        {
            m.Links.Add(new LinkDto("self", $"/api/movies/{m.Tconst}"));
        }
        return pageDto;
    }

    public async Task<PageDto<MovieDto>> GetSeriesTypesAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetSeriesTypesAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/movies/types/series");
        foreach (var m in pageDto.Items)
        {
            m.Links.Add(new LinkDto("self", $"/api/movies/{m.Tconst}"));
        }
        return pageDto;
    }

    public async Task<PageDto<MovieDto>> GetSpecialTypesAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetSpecialTypesAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/movies/types/special");
        foreach (var m in pageDto.Items)
        {
            m.Links.Add(new LinkDto("self", $"/api/movies/{m.Tconst}"));
        }
        return pageDto;
    }

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
