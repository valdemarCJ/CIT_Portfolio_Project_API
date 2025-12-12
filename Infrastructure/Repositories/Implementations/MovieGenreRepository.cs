using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

public class MovieGenreRepository : IMovieGenreRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MovieGenreRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MovieGenre>> GetGenresByMovieAsync(string tconst, CancellationToken ct = default)
    {
        return await _context.MovieGenres
            .Where(mg => mg.Tconst == tconst)
            .ToListAsync(ct);
    }

    public async Task<PageDto<MovieGenreDto>> GetMovieGenresAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var totalItems = await _context.MovieGenres.CountAsync(ct);
        var items = await _context.MovieGenres
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var dtos = _mapper.Map<List<MovieGenreDto>>(items);

        return new PageDto<MovieGenreDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            Total = totalItems,
            Links = new List<LinkDto>()
        };
    }
}