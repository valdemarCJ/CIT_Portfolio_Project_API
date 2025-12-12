using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class MovieGenreManager : IMovieGenreManager
{
    private readonly IMovieGenreRepository _repo;
    private readonly IMapper _mapper;

    public MovieGenreManager(IMovieGenreRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PageDto<MovieGenreDto>> GetMovieGenresAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetMovieGenresAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/movie-genres");
        foreach (var item in pageDto.Items)
        {
            item.Links.Add(new LinkDto("self", $"/api/movie-genres/{item.Tconst}/genres/{item.Genre}"));
        }
        return pageDto;
    }

    public async Task<List<MovieGenreDto>> GetGenresByMovieAsync(string tconst, CancellationToken ct = default)
    {
        var entities = await _repo.GetGenresByMovieAsync(tconst, ct);
        var dtos = _mapper.Map<List<MovieGenreDto>>(entities);
        foreach (var dto in dtos)
        {
            dto.Links.Add(new LinkDto("self", $"/api/movie-genres/{dto.Tconst}/genres/{dto.Genre}"));
        }
        return dtos;
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