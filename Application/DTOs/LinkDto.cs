namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Simple HATEOAS link (relation + href).
/// </summary>
public record LinkDto(string Rel, string Href);
