using Dapper;
using LMS.Application.Common.Exceptions;
using LMS.Application.Common.Pagination;
using LMS.Application.DTOs.Users;
using LMS.Domain.Services;
using LMS.Infrastructure.Dapper;
using MediatR;

namespace LMS.Application.Query.User;

public record GetMyProfileQuery(Guid UserId) : IRequest<UserProfileDto>;
public record GetAllUsersQuery(PagedRequest Request) : IRequest<PagedResult<UserSummaryDto>>;
public record GetUserByIdQuery(Guid UserId) : IRequest<UserProfileDto>;
public record GetInstructorsQuery : IRequest<IEnumerable<UserSummaryDto>>;

public class GetMyProfileHandler : IRequestHandler<GetMyProfileQuery, UserProfileDto>
{
    private readonly ISqlQueryService _db;
    public GetMyProfileHandler(ISqlQueryService db) => _db = db;

    public async Task<UserProfileDto> Handle(GetMyProfileQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT CAST(Id AS NVARCHAR(36)) AS Id, FullName, Email, AvatarUrl,
                   Role, IsActive, IsVerified, CreatedAt
            FROM AspNetUsers WHERE Id = @UserId
            """;
        return await _db.QueryFirstOrDefaultAsync<UserProfileDto>(sql, new { req.UserId }, ct)
            ?? throw new NotFoundException("User", req.UserId);
    }
}

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserProfileDto>
{
    private readonly ISqlQueryService _db;
    public GetUserByIdHandler(ISqlQueryService db) => _db = db;

    public async Task<UserProfileDto> Handle(GetUserByIdQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT CAST(Id AS NVARCHAR(36)) AS Id, FullName, Email, AvatarUrl,
                   Role, IsActive, IsVerified, CreatedAt
            FROM AspNetUsers WHERE Id = @UserId
            """;
        return await _db.QueryFirstOrDefaultAsync<UserProfileDto>(sql, new { req.UserId }, ct)
            ?? throw new NotFoundException("User", req.UserId);
    }
}

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserSummaryDto>>
{
    private readonly ISqlQueryService _db;
    public GetAllUsersHandler(ISqlQueryService db) => _db = db;

    public async Task<PagedResult<UserSummaryDto>> Handle(GetAllUsersQuery req, CancellationToken ct)
    {
        var r = req.Request; int offset = (r.Page - 1) * r.PageSize;
        var param = new DynamicParameters();
        param.Add("Offset", offset); param.Add("PageSize", r.PageSize);
        var where = "1=1";
        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            where = "(FullName LIKE @Search OR Email LIKE @Search)";
            param.Add("Search", $"%{r.Search}%");
        }
        var dataSql = $"SELECT CAST(Id AS NVARCHAR(36)) AS Id, FullName, Email, AvatarUrl, Role FROM AspNetUsers WHERE {where} ORDER BY CreatedAt DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        var countSql = $"SELECT COUNT(*) FROM AspNetUsers WHERE {where}";
        var (items, total) = await _db.QueryPagedAsync<UserSummaryDto>(dataSql, countSql, param, ct);
        return new PagedResult<UserSummaryDto> { Items = items, TotalCount = total, Page = r.Page, PageSize = r.PageSize };
    }
}

public class GetInstructorsHandler : IRequestHandler<GetInstructorsQuery, IEnumerable<UserSummaryDto>>
{
    private readonly ISqlQueryService _db;
    public GetInstructorsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<UserSummaryDto>> Handle(GetInstructorsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT CAST(Id AS NVARCHAR(36)) AS Id, FullName, Email, AvatarUrl, Role
            FROM AspNetUsers WHERE Role = 'Instructor' AND IsActive = 1 ORDER BY FullName
            """;
        return await _db.QueryAsync<UserSummaryDto>(sql, null, ct);
    }
}