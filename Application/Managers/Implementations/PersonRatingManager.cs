using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class PersonRatingManager : IPersonRatingManager
{
    private readonly IPersonRatingRepository _repo;
    private readonly IPersonRepository _personRepo;
    private readonly IMapper _mapper;

    public PersonRatingManager(IPersonRatingRepository repo, IPersonRepository personRepo, IMapper mapper)
    {
        _repo = repo;
        _personRepo = personRepo;
        _mapper = mapper;
    }

    public async Task RateAsync(int userId, string nconst, int value, CancellationToken ct = default)
    {
        // Verify person exists
        var person = await _personRepo.GetByIdAsync(nconst, ct);
        if (person == null)
            throw new ArgumentException($"Person {nconst} not found");

        await _repo.RateAsync(userId, nconst, value, ct);
    }

    public async Task<PageDto<PersonRatingDto>> GetRatingHistoryAsync(int userId, int page, int pageSize, CancellationToken ct = default)
    {
        var ratings = await _repo.GetUserPersonRatingHistoryAsync(userId, ct);

        var dtos = ratings.Select(r => new PersonRatingDto
        {
            Nconst = r.Nconst,
            Value = r.Value,
            Links = { new LinkDto("person", $"/api/people/{r.Nconst}") }
        }).ToList();

        var total = dtos.Count;
        var items = dtos.Skip((page - 1) * pageSize).Take(pageSize);

        return new PageDto<PersonRatingDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items,
            Links = { new LinkDto("self", $"/api/users/{userId}/history/person-ratings?page={page}&pageSize={pageSize}") }
        };
    }
}