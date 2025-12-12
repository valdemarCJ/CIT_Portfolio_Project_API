namespace CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

public class UserSearchHistoryRow
{
    public string Text { get; set; } = default!;
    public DateTime SearchedAt { get; set; }
}
