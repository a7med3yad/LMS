using Dapper;
using LMS.Application.Common.Pagination;
using LMS.Application.DTOs.Notifications;
using LMS.Domain.Models.Enums;
using LMS.Domain.Services;
using LMS.Infrastructure.Dapper;
using MediatR;

namespace LMS.Application.Query.Notification;

public record GetMyNotificationsQuery(Guid UserId, PagedRequest Request)
    : IRequest<PagedResult<NotificationDto>>;
public record GetUnreadCountQuery(Guid UserId) : IRequest<int>;

public class GetMyNotificationsHandler
    : IRequestHandler<GetMyNotificationsQuery, PagedResult<NotificationDto>>
{
    private readonly ISqlQueryService _db;
    public GetMyNotificationsHandler(ISqlQueryService db) => _db = db;

    public async Task<PagedResult<NotificationDto>> Handle(
        GetMyNotificationsQuery req, CancellationToken ct)
    {
        var r = req.Request; int offset = (r.Page - 1) * r.PageSize;
        var param = new DynamicParameters();
        param.Add("UserId", req.UserId); param.Add("Offset", offset); param.Add("PageSize", r.PageSize);

        const string data = "SELECT Id,TitleAr,TitleEn,BodyAr,BodyEn,Type,IsRead,ActionUrl,CourseId,ReferenceId,CreatedAt FROM Notifications WHERE UserId=@UserId ORDER BY CreatedAt DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        const string count = "SELECT COUNT(*) FROM Notifications WHERE UserId=@UserId";

        var (rows, total) = await _db.QueryPagedAsync<Row>(data, count, param, ct);
        return new PagedResult<NotificationDto>
        {
            Items = rows.Select(n => new NotificationDto(n.Id, n.TitleAr, n.TitleEn, n.BodyAr, n.BodyEn,
                Enum.Parse<NotificationType>(n.Type), n.IsRead, n.ActionUrl, n.CourseId, n.ReferenceId, n.CreatedAt)),
            TotalCount = total,
            Page = r.Page,
            PageSize = r.PageSize
        };
    }

    private record Row(Guid Id, string TitleAr, string TitleEn, string BodyAr, string BodyEn,
        string Type, bool IsRead, string? ActionUrl, Guid? CourseId, Guid? ReferenceId, DateTime CreatedAt);
}

public class GetUnreadCountHandler : IRequestHandler<GetUnreadCountQuery, int>
{
    private readonly ISqlQueryService _db;
    public GetUnreadCountHandler(ISqlQueryService db) => _db = db;

    public async Task<int> Handle(GetUnreadCountQuery req, CancellationToken ct)
        => await _db.QueryFirstOrDefaultAsync<int>(
            "SELECT COUNT(*) FROM Notifications WHERE UserId=@UserId AND IsRead=0",
            new { req.UserId }, ct);
}