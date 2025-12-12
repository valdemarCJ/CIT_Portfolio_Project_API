using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IPersonManager
{
    Task<PageDto<PersonDto>> GetPeopleAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PersonDto?> GetByIdAsync(string nconst, CancellationToken ct = default);
}
