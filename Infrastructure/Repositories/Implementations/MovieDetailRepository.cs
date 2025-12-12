using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

public class MovieDetailRepository : IMovieDetailRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MovieDetailRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MovieDetail?> GetByIdAsync(string tconst, CancellationToken ct = default)
    {
        return await _context.MovieDetails
            .FirstOrDefaultAsync(md => md.Tconst == tconst, ct);
    }

    public async Task<PageDto<MovieDetailDto>> GetMovieDetailsAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var totalItems = await _context.MovieDetails.CountAsync(ct);
        var items = await _context.MovieDetails
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var dtos = _mapper.Map<List<MovieDetailDto>>(items);

        return new PageDto<MovieDetailDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            Total = totalItems,
            Links = new List<LinkDto>()
        };
    }
}