using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class MovieDetailManager : IMovieDetailManager
{
    private readonly IMovieDetailRepository _repo;
    private readonly IMapper _mapper;

    public MovieDetailManager(IMovieDetailRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PageDto<MovieDetailDto>> GetMovieDetailsAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetMovieDetailsAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/movie-details");
        foreach (var item in pageDto.Items)
        {
            item.Links.Add(new LinkDto("self", $"/api/movie-details/{item.Tconst}"));
        }
        return pageDto;
    }

    public async Task<MovieDetailDto?> GetByIdAsync(string tconst, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(tconst, ct);
        if (entity == null) return null;
        var dto = _mapper.Map<MovieDetailDto>(entity);
        dto.Links.Add(new LinkDto("self", $"/api/movie-details/{dto.Tconst}"));
        return dto;
    }

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