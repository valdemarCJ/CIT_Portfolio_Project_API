namespace CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

public class BestMatchRow
{
    public string Tconst { get; set; } = default!;
    public int Rank { get; set; }
    public string Title { get; set; } = default!;
}
