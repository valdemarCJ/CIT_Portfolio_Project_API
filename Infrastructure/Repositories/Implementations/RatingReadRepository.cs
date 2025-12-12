using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

public class RatingReadRepository : IRatingReadRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public RatingReadRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Rating?> GetByIdAsync(string tconst, CancellationToken ct = default)
    {
        return await _context.Ratings
            .FirstOrDefaultAsync(r => r.Tconst == tconst, ct);
    }

    public async Task<PageDto<ImdbRatingDto>> GetRatingsAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var totalItems = await _context.Ratings.CountAsync(ct);
        var items = await _context.Ratings
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(r => r.Tconst)
            .ToListAsync(ct);

        var dtos = _mapper.Map<List<ImdbRatingDto>>(items);

        return new PageDto<ImdbRatingDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            Total = totalItems,
            Links = new List<LinkDto>()
        };
    }
}