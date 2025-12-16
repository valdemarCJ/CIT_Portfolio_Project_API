using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

/// <summary>
/// EF Core-backed person known for queries and projections.
/// </summary>
public class PersonKnownForRepository : IPersonKnownForRepository
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public PersonKnownForRepository(AppDbContext db, IMapper mapper)
    {
        _db = db; _mapper = mapper;
    }

    /// <summary>Returns a paged list of person known for entries using projection mapping.</summary>
    public async Task<PageDto<PersonKnownForDto>> GetPersonKnownForAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var query = _db.PersonKnownFor.AsQueryable();
        var total = await query.LongCountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .ProjectTo<PersonKnownForDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return new PageDto<PersonKnownForDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items
        };
    }

    /// <summary>Finds a person known for entry by nconst and tconst; null if not found.</summary>
    public async Task<PersonKnownFor?> GetByIdAsync(string nconst, string tconst, CancellationToken ct = default)
        => await _db.PersonKnownFor.FirstOrDefaultAsync(p => p.Nconst == nconst && p.Tconst == tconst, ct);

    /// <summary>Returns tconst values for a given nconst (movies known for a person).</summary>
    public async Task<List<string>> GetMoviesByPersonAsync(string nconst, CancellationToken ct = default)
        => await _db.PersonKnownFor.Where(p => p.Nconst == nconst).Select(p => p.Tconst).ToListAsync(ct);

    /// <summary>Returns nconst values for a given tconst (people known for a movie).</summary>
    public async Task<List<string>> GetPeopleByMovieAsync(string tconst, CancellationToken ct = default)
        => await _db.PersonKnownFor.Where(p => p.Tconst == tconst).Select(p => p.Nconst).ToListAsync(ct);
}