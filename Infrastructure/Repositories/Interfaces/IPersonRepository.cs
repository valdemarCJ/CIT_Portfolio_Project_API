using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for People resources (read-only list/detail projections).
/// </summary>
public interface IPersonRepository
{
    /// <summary>Returns a paged list of people.</summary>
    Task<PageDto<PersonDto>> GetPeopleAsync(int page, int pageSize, CancellationToken ct = default);
    /// <summary>Finds a person entity by nconst; null if not found.</summary>
    Task<Person?> GetByIdAsync(string nconst, CancellationToken ct = default);
}
