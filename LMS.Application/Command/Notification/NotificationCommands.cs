using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Domain.Models;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Notification;

public record DeleteNotificationCommand(Guid NotificationId, Guid UserId) : IRequest;

public record NotifyNewMaterialCommand(Guid CourseId, Guid MaterialId) : IRequest;
public record NotifyNewAssignmentCommand(Guid CourseId, Guid AssignmentId) : IRequest;
public record NotifyNewExamCommand(Guid CourseId, Guid ExamId) : IRequest;
public record NotifyAssignmentGradedCommand(Guid StudentId, Guid SubmissionId) : IRequest;
public record NotifyCourseUpdateCommand(Guid CourseId, string MessageAr, string MessageEn) : IRequest;
public record SendGeneralNotificationCommand(Guid UserId, string TitleAr, string TitleEn, string BodyAr, string BodyEn) : IRequest;

// ─── Delete ───
public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteNotificationCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteNotificationCommand req, CancellationToken ct)
    {
        var n = await _uow.Notifications.GetByIdAsync(req.NotificationId, ct)
            ?? throw new NotFoundException("Notification", req.NotificationId);
        if (n.UserId != req.UserId) throw new ForbiddenException();
        _uow.Notifications.Remove(n);
        await _uow.SaveChangesAsync(ct);
    }
}

// ─── Notify New Material ───
public class NotifyNewMaterialHandler : IRequestHandler<NotifyNewMaterialCommand>
{
    private readonly IUnitOfWork _uow;
    public NotifyNewMaterialHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(NotifyNewMaterialCommand req, CancellationToken ct)
    {
        var enrollments = await _uow.Enrollments.GetEnrollmentsByCourseAsync(req.CourseId, ct);
        var notifications = enrollments
            .Where(e => e.Status == EnrollmentStatus.Active)
            .Select(e => new Domain.Models.Notification
            {
                Id = Guid.NewGuid(),
                TitleAr = "مادة جديدة",
                TitleEn = "New Material",
                BodyAr = "تم إضافة مادة جديدة للدورة",
                BodyEn = "New material has been added to the course.",
                Type = NotificationType.NewMaterial,
                CourseId = req.CourseId,
                ReferenceId = req.MaterialId,
                UserId = e.StudentId
            });
        await _uow.Notifications.AddRangeAsync(notifications, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

// ─── Notify New Assignment ───
public class NotifyNewAssignmentHandler : IRequestHandler<NotifyNewAssignmentCommand>
{
    private readonly IUnitOfWork _uow;
    public NotifyNewAssignmentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(NotifyNewAssignmentCommand req, CancellationToken ct)
    {
        var enrollments = await _uow.Enrollments.GetEnrollmentsByCourseAsync(req.CourseId, ct);
        var notifications = enrollments
            .Where(e => e.Status == EnrollmentStatus.Active)
            .Select(e => new Domain.Models.Notification
            {
                Id = Guid.NewGuid(),
                TitleAr = "واجب جديد",
                TitleEn = "New Assignment",
                BodyAr = "تم إضافة واجب جديد للدورة",
                BodyEn = "A new assignment has been added to the course.",
                Type = NotificationType.NewAssignment,
                CourseId = req.CourseId,
                ReferenceId = req.AssignmentId,
                UserId = e.StudentId
            });
        await _uow.Notifications.AddRangeAsync(notifications, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

// ─── Notify New Exam ───
public class NotifyNewExamHandler : IRequestHandler<NotifyNewExamCommand>
{
    private readonly IUnitOfWork _uow;
    public NotifyNewExamHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(NotifyNewExamCommand req, CancellationToken ct)
    {
        var enrollments = await _uow.Enrollments.GetEnrollmentsByCourseAsync(req.CourseId, ct);
        var notifications = enrollments
            .Where(e => e.Status == EnrollmentStatus.Active)
            .Select(e => new Domain.Models.Notification
            {
                Id = Guid.NewGuid(),
                TitleAr = "اختبار جديد",
                TitleEn = "New Exam",
                BodyAr = "تم إضافة اختبار جديد للدورة",
                BodyEn = "A new exam has been added to the course.",
                Type = NotificationType.NewExam,
                CourseId = req.CourseId,
                ReferenceId = req.ExamId,
                UserId = e.StudentId
            });
        await _uow.Notifications.AddRangeAsync(notifications, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

// ─── Notify Assignment Graded ───
public class NotifyAssignmentGradedHandler : IRequestHandler<NotifyAssignmentGradedCommand>
{
    private readonly IUnitOfWork _uow;
    public NotifyAssignmentGradedHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(NotifyAssignmentGradedCommand req, CancellationToken ct)
    {
        var notification = new Domain.Models.Notification
        {
            Id = Guid.NewGuid(),
            TitleAr = "تم تقييم الواجب",
            TitleEn = "Assignment Graded",
            BodyAr = "تم تقييم واجبك",
            BodyEn = "Your assignment has been graded.",
            Type = NotificationType.AssignmentGraded,
            ReferenceId = req.SubmissionId,
            UserId = req.StudentId
        };
        await _uow.Notifications.AddAsync(notification, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

// ─── Notify Course Update ───
public class NotifyCourseUpdateHandler : IRequestHandler<NotifyCourseUpdateCommand>
{
    private readonly IUnitOfWork _uow;
    public NotifyCourseUpdateHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(NotifyCourseUpdateCommand req, CancellationToken ct)
    {
        var enrollments = await _uow.Enrollments.GetEnrollmentsByCourseAsync(req.CourseId, ct);
        var notifications = enrollments
            .Where(e => e.Status == EnrollmentStatus.Active)
            .Select(e => new Domain.Models.Notification
            {
                Id = Guid.NewGuid(),
                TitleAr = "تحديث الدورة",
                TitleEn = "Course Update",
                BodyAr = req.MessageAr,
                BodyEn = req.MessageEn,
                Type = NotificationType.CourseUpdate,
                CourseId = req.CourseId,
                UserId = e.StudentId
            });
        await _uow.Notifications.AddRangeAsync(notifications, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

// ─── Send General Notification ───
public class SendGeneralNotificationHandler : IRequestHandler<SendGeneralNotificationCommand>
{
    private readonly IUnitOfWork _uow;
    public SendGeneralNotificationHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(SendGeneralNotificationCommand req, CancellationToken ct)
    {
        var notification = new Domain.Models.Notification
        {
            Id = Guid.NewGuid(),
            TitleAr = req.TitleAr,
            TitleEn = req.TitleEn,
            BodyAr = req.BodyAr,
            BodyEn = req.BodyEn,
            Type = NotificationType.General,
            UserId = req.UserId
        };
        await _uow.Notifications.AddAsync(notification, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
