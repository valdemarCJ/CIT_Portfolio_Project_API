using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/sql")]
[Authorize] // Fjern kommentaren for at kræve login/bearer token
public class SqlController : ControllerBase
{
    private readonly AppDbContext _db;
    public SqlController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
        /// Execute a custom SQL query and get the result as a JSON table
    /// </summary>
        /// <param name="sql">The SQL query to execute</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>JSON object with columns and rows</returns>
    [HttpPost("query")]
        public async Task<IActionResult> Query(
            [FromQuery][Required] string sql,
            CancellationToken ct)
    {
            if (string.IsNullOrWhiteSpace(sql))
            return BadRequest("SQL query required");
        try
        {
            await using var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync(ct);
            await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
            using var reader = await cmd.ExecuteReaderAsync(ct);
            var table = new List<Dictionary<string, object?>>();
            var cols = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            while (await reader.ReadAsync(ct))
            {
                var row = new Dictionary<string, object?>();
                foreach (var col in cols)
                    row[col] = reader[col];
                table.Add(row);
            }
            return Ok(new { columns = cols, rows = table });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    public record SqlQueryRequest(string Sql);
}
