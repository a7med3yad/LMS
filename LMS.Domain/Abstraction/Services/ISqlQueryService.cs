namespace LMS.Domain.Services;

public interface ISqlQueryService
{
    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? param = null,
        CancellationToken ct = default);

    Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? param = null,
        CancellationToken ct = default);

    /// <summary>Runs dataSql and countSql in ONE round trip. Returns (items, total).</summary>
    Task<(IEnumerable<T> Items, int Total)> QueryPagedAsync<T>(
        string dataSql,
        string countSql,
        object? param = null,
        CancellationToken ct = default);

    /// <summary>For multi-result queries (e.g. attempt + answers together).</summary>
    Task UseConnectionAsync(Func<System.Data.IDbConnection, Task> action);
}