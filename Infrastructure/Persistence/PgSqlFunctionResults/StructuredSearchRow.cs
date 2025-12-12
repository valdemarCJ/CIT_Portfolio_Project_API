namespace CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

// Result row of structured_string_search(userId, ...)
public class StructuredSearchRow
{
    public string Tconst { get; set; } = default!;
    public string Title { get; set; } = default!;
}
