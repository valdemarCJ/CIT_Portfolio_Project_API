using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class PersonBookmarkManager : IPersonBookmarkManager
{
    private readonly IPersonBookmarkRepository _repo;
    private readonly IPersonRepository _personRepo;
    private readonly IMapper _mapper;

    public PersonBookmarkManager(IPersonBookmarkRepository repo, IPersonRepository personRepo, IMapper mapper)
    {
        _repo = repo;
        _personRepo = personRepo;
        _mapper = mapper;
    }

    public async Task AddAsync(int userId, string nconst, string? note, CancellationToken ct = default)
    {
        // Verify person exists
        var person = await _personRepo.GetByIdAsync(nconst, ct);
        if (person == null)
            throw new ArgumentException($"Person {nconst} not found");

        await _repo.AddAsync(userId, nconst, note, ct);
    }

    public async Task<PageDto<PersonBookmarkDto>> GetAsync(int userId, int page, int pageSize, CancellationToken ct = default)
    {
        var bookmarks = await _repo.GetUserPersonBookmarksAsync(userId, ct);
        
        var dtos = new List<PersonBookmarkDto>();
        foreach (var bookmark in bookmarks)
        {
            var person = await _personRepo.GetByIdAsync(bookmark.Nconst, ct);
            var dto = new PersonBookmarkDto
            {
                Nconst = bookmark.Nconst,
                Name = person?.Name,
                Note = bookmark.Note
            };
            dto.Links.Add(new LinkDto("self", $"/api/people/{bookmark.Nconst}"));
            dtos.Add(dto);
        }

        var total = dtos.Count;
        var items = dtos.Skip((page - 1) * pageSize).Take(pageSize);

        return new PageDto<PersonBookmarkDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items,
            Links = { new LinkDto("self", $"/api/users/{userId}/person-bookmarks?page={page}&pageSize={pageSize}") }
        };
    }

    public async Task DeleteAsync(int userId, string nconst, CancellationToken ct = default)
    {
        await _repo.DeleteAsync(userId, nconst, ct);
    }
}