namespace CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

public class UserRatingHistoryRow
{
    public string Tconst { get; set; } = default!;
    public int Value { get; set; }
    public DateTime? RatedAt { get; set; }
}
