using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

/// <summary>
/// EF Core-backed movie queries and projections.
/// </summary>
public class MovieRepository : IMovieRepository
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public MovieRepository(AppDbContext db, IMapper mapper)
    {
        _db = db; _mapper = mapper;
    }

    /// <summary>Returns a paged list of movies using projection mapping.</summary>
    public async Task<PageDto<MovieDto>> GetMoviesAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var query = _db.Movies.AsQueryable();
        var total = await query.LongCountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return new PageDto<MovieDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items
        };
    }

    /// <summary>Finds a movie by tconst; null if not found.</summary>
    public async Task<Movie?> GetByIdAsync(string tconst, CancellationToken ct = default)
        => await _db.Movies.FirstOrDefaultAsync(m => m.Tconst == tconst, ct);

    /// <summary>Returns paged movies filtered by movie types (movie, tvMovie, short).</summary>
    public async Task<PageDto<MovieDto>> GetMovieTypesAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var movieTypes = new[] { "movie", "tvMovie", "short" };
        var query = _db.Movies.Where(m => movieTypes.Contains(m.TitleType));
        var total = await query.LongCountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return new PageDto<MovieDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items
        };
    }

    /// <summary>Returns paged movies filtered by series types (tvMiniSeries, tvEpisode, tvSeries).</summary>
    public async Task<PageDto<MovieDto>> GetSeriesTypesAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var seriesTypes = new[] { "tvMiniSeries", "tvEpisode", "tvSeries" };
        var query = _db.Movies.Where(m => seriesTypes.Contains(m.TitleType));
        var total = await query.LongCountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return new PageDto<MovieDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items
        };
    }

    /// <summary>Returns paged movies filtered by special types (tvShort, videoGame, video, tvSpecial).</summary>
    public async Task<PageDto<MovieDto>> GetSpecialTypesAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var specialTypes = new[] { "tvShort", "videoGame", "video", "tvSpecial" };
        var query = _db.Movies.Where(m => specialTypes.Contains(m.TitleType));
        var total = await query.LongCountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return new PageDto<MovieDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items
        };
    }
}
