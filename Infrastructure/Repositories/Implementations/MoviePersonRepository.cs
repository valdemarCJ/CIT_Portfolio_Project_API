using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

public class MoviePersonRepository : IMoviePersonRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MoviePersonRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MoviePerson>> GetPeopleByMovieAsync(string tconst, CancellationToken ct = default)
    {
        return await _context.MoviePeople
            .Where(mp => mp.Tconst == tconst)
            .OrderBy(mp => mp.Ordering)
            .ToListAsync(ct);
    }

    public async Task<PageDto<MoviePersonDto>> GetMoviePeopleAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var totalItems = await _context.MoviePeople.CountAsync(ct);
        var items = await _context.MoviePeople
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(mp => mp.Tconst)
            .ThenBy(mp => mp.Ordering)
            .ToListAsync(ct);

        var dtos = _mapper.Map<List<MoviePersonDto>>(items);

        return new PageDto<MoviePersonDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            Total = totalItems,
            Links = new List<LinkDto>()
        };
    }
}