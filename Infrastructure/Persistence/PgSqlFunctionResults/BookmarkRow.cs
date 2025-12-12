namespace CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

// Result row of get_bookmarks(userId)
public class BookmarkRow
{
    public string Tconst { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Note { get; set; }
}
