namespace CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

// Result row of string_search(userId, text)
public class SearchRow
{
    public string Tconst { get; set; } = default!;
    public string Title { get; set; } = default!;
}
