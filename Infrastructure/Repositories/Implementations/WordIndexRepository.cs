using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

public class WordIndexRepository : IWordIndexRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public WordIndexRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WordIndex>> GetWordsByMovieAsync(string tconst, CancellationToken ct = default)
    {
        return await _context.WordIndex
            .Where(wi => wi.Tconst == tconst)
            .OrderBy(wi => wi.Word)
            .ToListAsync(ct);
    }

    public async Task<PageDto<WordIndexDto>> GetWordIndexAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var totalItems = await _context.WordIndex.CountAsync(ct);
        var items = await _context.WordIndex
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(wi => wi.Tconst)
            .ThenBy(wi => wi.Word)
            .ToListAsync(ct);

        var dtos = _mapper.Map<List<WordIndexDto>>(items);

        return new PageDto<WordIndexDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            Total = totalItems,
            Links = new List<LinkDto>()
        };
    }
}