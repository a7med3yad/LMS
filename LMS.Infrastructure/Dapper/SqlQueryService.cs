using Dapper;
using LMS.Domain.Services;
using System.Data;

namespace LMS.Infrastructure.Dapper;

public class SqlQueryService : ISqlQueryService
{
    private readonly DapperContext _ctx;

    public SqlQueryService(DapperContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql, object? param = null, CancellationToken ct = default)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.QueryAsync<T>(
            new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql, object? param = null, CancellationToken ct = default)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<T>(
            new CommandDefinition(sql, param, cancellationToken: ct));
    }

    public async Task<(IEnumerable<T> Items, int Total)> QueryPagedAsync<T>(
        string dataSql, string countSql,
        object? param = null, CancellationToken ct = default)
    {
        using var conn = _ctx.CreateConnection();
        var multi = await conn.QueryMultipleAsync($"{dataSql}; {countSql}", param);
        var items = await multi.ReadAsync<T>();
        var total = await multi.ReadFirstAsync<int>();
        return (items, total);
    }

    public async Task UseConnectionAsync(Func<IDbConnection, Task> action)
    {
        using var conn = _ctx.CreateConnection();
        await action(conn);
    }
}