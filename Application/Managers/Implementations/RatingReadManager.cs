using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class RatingReadManager : IRatingReadManager
{
    private readonly IRatingReadRepository _repo;
    private readonly IMapper _mapper;

    public RatingReadManager(IRatingReadRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PageDto<ImdbRatingDto>> GetRatingsAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pageDto = await _repo.GetRatingsAsync(page, pageSize, ct);
        AddPageLinks(pageDto, "/api/imdb-ratings");
        foreach (var item in pageDto.Items)
        {
            item.Links.Add(new LinkDto("self", $"/api/imdb-ratings/{item.Tconst}"));
        }
        return pageDto;
    }

    public async Task<ImdbRatingDto?> GetByIdAsync(string tconst, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(tconst, ct);
        if (entity == null) return null;
        var dto = _mapper.Map<ImdbRatingDto>(entity);
        dto.Links.Add(new LinkDto("self", $"/api/imdb-ratings/{dto.Tconst}"));
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