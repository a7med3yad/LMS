# LMS — Clean Architecture Design
**ASP.NET Core Web API · Production-Grade · API-First**

---

## IMPLEMENTATION STATUS

| Module | Status | Notes |
|---|---|---|
| ✅ Auth | **Done** | Register, OTP verify, login, forgot/reset password, refresh token, logout, change password, OAuth (Google/Facebook), assign role |
| ⬜ Users | Pending | |
| ⬜ Courses | Pending | |
| ⬜ Materials | Pending | |
| ⬜ Assignments | Pending | |
| ⬜ Exams | Pending | |
| ⬜ Enrollments | Pending | |
| ⬜ Payments | Pending | |
| ⬜ Vouchers | Pending | |
| ⬜ Notifications | Pending | |

---

## 1. FOLDER STRUCTURE

```
LMS/
├── LMS.csproj
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
│
├── Common/                              # Cross-cutting concerns
│   ├── Constants/
│   │   ├── Roles.cs
│   │   ├── Policies.cs
│   │   └── CacheKeys.cs
│   ├── Exceptions/
│   │   ├── NotFoundException.cs
│   │   ├── ForbiddenException.cs
│   │   ├── ConflictException.cs
│   │   ├── ValidationException.cs
│   │   └── PaymentException.cs
│   ├── Pagination/
│   │   ├── PagedRequest.cs
│   │   └── PagedResult.cs
│   ├── Result/
│   │   └── Result.cs                    # Generic Result<T> wrapper
│   ├── EmailSettings.cs
│   └── StripeSettings.cs
│
├── Models/                              # Domain Entities (already implemented)
│   ├── ApplicationUser.cs
│   ├── Course.cs
│   ├── Material.cs
│   ├── Assignment.cs
│   ├── AssignmentSubmission.cs
│   ├── Exam.cs
│   ├── Question.cs
│   ├── QuestionChoice.cs
│   ├── ExamAttempt.cs
│   ├── ExamAnswer.cs
│   ├── Enrollment.cs
│   ├── Voucher.cs
│   ├── Notification.cs
│   ├── RefreshToken.cs
│   └── Enums/
│       ├── UserRole.cs
│       ├── CourseStatus.cs
│       ├── EnrollmentStatus.cs
│       ├── MaterialType.cs
│       ├── NotificationType.cs
│       ├── QuestionType.cs
│       └── SubmissionType.cs
│
├── DTOs/                                # Request / Response models
│   ├── Auth/
│   │   ├── RegisterDto.cs
│   │   ├── LoginDto.cs
│   │   ├── VerifyOtpDto.cs
│   │   ├── ResendOtpDto.cs
│   │   ├── ForgotPasswordDto.cs
│   │   ├── ResetForgetPasswordDto.cs
│   │   ├── ChangePasswordDto.cs
│   │   ├── RefreshTokenDto.cs
│   │   ├── AuthResponseDto.cs
│   │   └── AssignRoleDto.cs
│   ├── Users/
│   │   ├── UserProfileDto.cs
│   │   ├── UpdateProfileDto.cs
│   │   ├── UserSummaryDto.cs
│   │   └── UpdateAvatarDto.cs
│   ├── Courses/
│   │   ├── CreateCourseDto.cs
│   │   ├── UpdateCourseDto.cs
│   │   ├── CourseDto.cs
│   │   ├── CourseSummaryDto.cs
│   │   └── CourseFilterDto.cs
│   ├── Materials/
│   │   ├── CreateMaterialDto.cs
│   │   ├── UpdateMaterialDto.cs
│   │   └── MaterialDto.cs
│   ├── Assignments/
│   │   ├── CreateAssignmentDto.cs
│   │   ├── UpdateAssignmentDto.cs
│   │   ├── AssignmentDto.cs
│   │   ├── SubmitAssignmentDto.cs
│   │   ├── GradeSubmissionDto.cs
│   │   └── AssignmentSubmissionDto.cs
│   ├── Exams/
│   │   ├── CreateExamDto.cs
│   │   ├── UpdateExamDto.cs
│   │   ├── ExamDto.cs
│   │   ├── CreateQuestionDto.cs
│   │   ├── UpdateQuestionDto.cs
│   │   ├── QuestionDto.cs
│   │   ├── QuestionChoiceDto.cs
│   │   ├── StartExamResponseDto.cs
│   │   ├── SubmitExamDto.cs
│   │   ├── ExamAnswerDto.cs
│   │   ├── ExamAttemptDto.cs
│   │   └── GradeOpenEndedDto.cs
│   ├── Enrollments/
│   │   ├── EnrollRequestDto.cs
│   │   └── EnrollmentDto.cs
│   ├── Payments/
│   │   ├── CreatePaymentIntentDto.cs
│   │   ├── ConfirmPaymentDto.cs
│   │   ├── PaymentIntentResponseDto.cs
│   │   └── ApplyVoucherDto.cs
│   ├── Vouchers/
│   │   ├── CreateVoucherDto.cs
│   │   ├── UpdateVoucherDto.cs
│   │   └── VoucherDto.cs
│   └── Notifications/
│       ├── NotificationDto.cs
│       └── MarkReadDto.cs
│
├── Repositories/                        # Data access abstraction
│   ├── Interfaces/
│   │   ├── IRepository.cs
│   │   ├── ICourseRepository.cs
│   │   ├── IEnrollmentRepository.cs
│   │   ├── IUserRepository.cs
│   │   ├── IPaymentRepository.cs
│   │   ├── IAssignmentRepository.cs
│   │   └── IExamRepository.cs
│   ├── IUnitOfWork.cs
│   ├── Repository.cs                    # Generic implementation
│   └── UnitOfWork.cs                    # Implementation
│
├── Application/
│   └── Services/
│       ├── Interfaces/
│       │   ├── IAuthService.cs                  # ✅ IMPLEMENTED
│       │   ├── IUserService.cs
│       │   ├── ICourseService.cs
│       │   ├── IMaterialService.cs
│       │   ├── IAssignmentService.cs
│       │   ├── IExamService.cs
│       │   ├── IEnrollmentService.cs
│       │   ├── IPaymentService.cs
│       │   ├── IVoucherService.cs
│       │   └── INotificationService.cs
│       ├── AuthServices/
│       │   └── AuthService.cs                   # ✅ IMPLEMENTED
│       ├── CourseService.cs
│       ├── MaterialService.cs
│       ├── AssignmentService.cs
│       ├── ExamService.cs
│       ├── EnrollmentService.cs
│       ├── PaymentService.cs
│       ├── VoucherService.cs
│       ├── NotificationService.cs
│       └── UserService.cs
│
├── Infrastructure/
│   └── Services/
│       ├── TokenService.cs                      # ✅ IMPLEMENTED
│       ├── OtpService.cs                        # ✅ IMPLEMENTED
│       ├── EmailService.cs                      # ✅ IMPLEMENTED
│       ├── OAuthService.cs                      # ✅ IMPLEMENTED
│       └── StripeService.cs
│
├── Data/
│   ├── AppDbContext.cs
│   └── Configurations/
│       ├── AppUserConfiguration.cs
│       ├── CourseConfiguration.cs
│       ├── MaterialConfiguration.cs
│       ├── AssignmentConfiguration.cs
│       ├── AssignmentSubmissionConfiguration.cs
│       ├── ExamConfiguration.cs
│       ├── QuestionConfiguration.cs
│       ├── ExamAttemptConfiguration.cs
│       ├── EnrollmentConfiguration.cs
│       ├── VoucherConfiguration.cs
│       ├── NotificationConfiguration.cs
│       └── RefreshTokenConfiguration.cs
│
└── Controllers/
    └── v1/
        ├── AuthController.cs                    # ✅ IMPLEMENTED
        ├── UsersController.cs
        ├── CoursesController.cs
        ├── MaterialsController.cs
        ├── AssignmentsController.cs
        ├── ExamsController.cs
        ├── EnrollmentsController.cs
        ├── PaymentsController.cs
        ├── VouchersController.cs
        └── NotificationsController.cs
```

---

## 2. DTOS

### 2.1 Common Utilities

```csharp
// Common/Pagination/PagedRequest.cs
public class PagedRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
}

// Common/Pagination/PagedResult.cs
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext => Page < TotalPages;
    public bool HasPrevious => Page > 1;
}

// Common/Result/Result.cs
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; }

    public static Result<T> Success(T data, int statusCode = 200) => new() { IsSuccess = true, Data = data, StatusCode = statusCode };
    public static Result<T> Failure(string error, int statusCode = 400) => new() { IsSuccess = false, Error = error, StatusCode = statusCode };
}
```

---

### 2.2 Auth DTOs  ✅ IMPLEMENTED

```csharp
// DTOs/Auth/RegisterDto.cs
public record RegisterDto(
    [Required] string FullName,
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)] string Password,
    [Required] UserRole Role
);

// DTOs/Auth/LoginDto.cs
public record LoginDto(
    [Required][EmailAddress] string Email,
    [Required] string Password
);

// DTOs/Auth/VerifyOtpDto.cs
public record VerifyOtpDto(
    [Required][EmailAddress] string Email,
    [Required][Length(6, 6)] string Otp
);

// DTOs/Auth/ResendOtpDto.cs
public record ResendOtpDto([Required][EmailAddress] string Email);

// DTOs/Auth/ForgotPasswordDto.cs
public record ForgotPasswordDto([Required][EmailAddress] string Email);

// DTOs/Auth/ResetForgetPasswordDto.cs
public record ResetForgetPasswordDto(
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)] string NewPassword
);

// DTOs/Auth/ChangePasswordDto.cs
public record ChangePasswordDto(
    [Required][MinLength(8)] string CurrentPassword,
    [Required][MinLength(8)] string NewPassword
);

// DTOs/Auth/RefreshTokenDto.cs
public record RefreshTokenDto([Required] string RefreshToken);

// DTOs/Auth/AssignRoleDto.cs
public record AssignRoleDto(
    [Required][EmailAddress] string Email,
    [Required] string Role
);

// DTOs/Auth/AuthResponseDto.cs
public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    string UserId,
    string FullName,
    string Email,
    string Role
);
```

---

### 2.3 User DTOs

```csharp
// DTOs/Users/UserProfileDto.cs
public record UserProfileDto(
    string Id,
    string FullName,
    string Email,
    string? AvatarUrl,
    string Role,
    bool IsActive,
    bool IsVerified,
    DateTime CreatedAt
);

// DTOs/Users/UpdateProfileDto.cs
public record UpdateProfileDto(
    [Required][MaxLength(200)] string FullName
);

// DTOs/Users/UpdateAvatarDto.cs
public record UpdateAvatarDto(
    [Required] string AvatarUrl
);

// DTOs/Users/UserSummaryDto.cs
public record UserSummaryDto(
    string Id,
    string FullName,
    string Email,
    string? AvatarUrl,
    string Role
);
```

---

### 2.4 Course DTOs

```csharp
// DTOs/Courses/CreateCourseDto.cs
public record CreateCourseDto(
    [Required][MaxLength(300)] string TitleAr,
    [Required][MaxLength(300)] string TitleEn,
    [MaxLength(5000)] string? DescriptionAr,
    [MaxLength(5000)] string? DescriptionEn,
    string? ThumbnailUrl,
    [Range(0, double.MaxValue)] decimal Price
);

// DTOs/Courses/UpdateCourseDto.cs
public record UpdateCourseDto(
    [MaxLength(300)] string? TitleAr,
    [MaxLength(300)] string? TitleEn,
    [MaxLength(5000)] string? DescriptionAr,
    [MaxLength(5000)] string? DescriptionEn,
    string? ThumbnailUrl,
    decimal? Price,
    CourseStatus? Status
);

// DTOs/Courses/CourseDto.cs
public record CourseDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    string? ThumbnailUrl,
    decimal Price,
    CourseStatus Status,
    UserSummaryDto Instructor,
    int EnrollmentCount,
    int MaterialCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

// DTOs/Courses/CourseSummaryDto.cs
public record CourseSummaryDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string? ThumbnailUrl,
    decimal Price,
    CourseStatus Status,
    string InstructorName,
    int EnrollmentCount,
    DateTime CreatedAt
);

// DTOs/Courses/CourseFilterDto.cs
public class CourseFilterDto : PagedRequest
{
    public CourseStatus? Status { get; set; }
    public Guid? InstructorId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
```

---

### 2.5 Material DTOs

```csharp
// DTOs/Materials/CreateMaterialDto.cs
public record CreateMaterialDto(
    [Required][MaxLength(300)] string TitleAr,
    [Required][MaxLength(300)] string TitleEn,
    [MaxLength(2000)] string? DescriptionAr,
    [MaxLength(2000)] string? DescriptionEn,
    [Required] MaterialType Type,
    [Required] string ContentUrl,
    string? TextContent,
    int Order
);

// DTOs/Materials/UpdateMaterialDto.cs
public record UpdateMaterialDto(
    [MaxLength(300)] string? TitleAr,
    [MaxLength(300)] string? TitleEn,
    [MaxLength(2000)] string? DescriptionAr,
    [MaxLength(2000)] string? DescriptionEn,
    MaterialType? Type,
    string? ContentUrl,
    string? TextContent,
    int? Order,
    bool? IsPublished
);

// DTOs/Materials/MaterialDto.cs
public record MaterialDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    MaterialType Type,
    string ContentUrl,
    string? TextContent,
    int Order,
    bool IsPublished,
    Guid CourseId,
    DateTime CreatedAt
);
```

---

### 2.6 Assignment DTOs

```csharp
// DTOs/Assignments/CreateAssignmentDto.cs
public record CreateAssignmentDto(
    [Required][MaxLength(300)] string TitleAr,
    [Required][MaxLength(300)] string TitleEn,
    [Required] string DescriptionAr,
    [Required] string DescriptionEn,
    [Required] SubmissionType SubmissionType,
    [Required] DateTime DeadLine,
    [Range(1, 1000)] int MaxGrade
);

// DTOs/Assignments/UpdateAssignmentDto.cs
public record UpdateAssignmentDto(
    [MaxLength(300)] string? TitleAr,
    [MaxLength(300)] string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    SubmissionType? SubmissionType,
    DateTime? DeadLine,
    int? MaxGrade,
    bool? IsPublished
);

// DTOs/Assignments/AssignmentDto.cs
public record AssignmentDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string DescriptionAr,
    string DescriptionEn,
    SubmissionType SubmissionType,
    DateTime DeadLine,
    int MaxGrade,
    bool IsPublished,
    Guid CourseId,
    DateTime CreatedAt
);

// DTOs/Assignments/SubmitAssignmentDto.cs
public record SubmitAssignmentDto(
    string? TextAnswer,
    string? FileUrl
);

// DTOs/Assignments/GradeSubmissionDto.cs
public record GradeSubmissionDto(
    [Required][Range(0, 1000)] int Grade,
    string? Feedback
);

// DTOs/Assignments/AssignmentSubmissionDto.cs
public record AssignmentSubmissionDto(
    Guid Id,
    Guid AssignmentId,
    string AssignmentTitle,
    UserSummaryDto Student,
    string? TextAnswer,
    string? FileUrl,
    int Grade,
    string? Feedback,
    DateTime SubmittedAt,
    DateTime? GradedAt
);
```

---

### 2.7 Exam DTOs

```csharp
// DTOs/Exams/CreateExamDto.cs
public record CreateExamDto(
    [Required][MaxLength(300)] string TitleAr,
    [Required][MaxLength(300)] string TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    [Required][Range(1, 600)] int DurationMinutes,
    [Required][Range(0, 100)] int PassScore,
    [Range(1, 10)] int MaxAttempts,
    DateTime? AvailableFrom,
    DateTime? AvailableUntil
);

// DTOs/Exams/UpdateExamDto.cs
public record UpdateExamDto(
    [MaxLength(300)] string? TitleAr,
    [MaxLength(300)] string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    int? DurationMinutes,
    int? PassScore,
    int? MaxAttempts,
    bool? IsPublished,
    DateTime? AvailableFrom,
    DateTime? AvailableUntil
);

// DTOs/Exams/ExamDto.cs
public record ExamDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    int DurationMinutes,
    int PassScore,
    int MaxAttempts,
    bool IsPublished,
    DateTime? AvailableFrom,
    DateTime? AvailableUntil,
    Guid CourseId,
    int QuestionCount,
    DateTime CreatedAt
);

// DTOs/Exams/CreateQuestionDto.cs
public record CreateQuestionDto(
    [Required][MaxLength(2000)] string TextAr,
    [Required][MaxLength(2000)] string TextEn,
    [Required] QuestionType Type,
    [Range(1, 100)] int Points,
    int Order,
    IEnumerable<CreateQuestionChoiceDto>? Choices   // Required if MultipleChoice or TrueFalse
);

// DTOs/Exams/CreateQuestionChoiceDto.cs
public record CreateQuestionChoiceDto(
    [Required][MaxLength(1000)] string TextAr,
    [Required][MaxLength(1000)] string TextEn,
    bool IsCorrect
);

// DTOs/Exams/UpdateQuestionDto.cs
public record UpdateQuestionDto(
    [MaxLength(2000)] string? TextAr,
    [MaxLength(2000)] string? TextEn,
    QuestionType? Type,
    int? Points,
    int? Order
);

// DTOs/Exams/QuestionChoiceDto.cs
public record QuestionChoiceDto(
    Guid Id,
    string TextAr,
    string TextEn,
    bool? IsCorrect   // Null when returned to student during attempt
);

// DTOs/Exams/QuestionDto.cs
public record QuestionDto(
    Guid Id,
    string TextAr,
    string TextEn,
    QuestionType Type,
    int Points,
    int Order,
    IEnumerable<QuestionChoiceDto> Choices
);

// DTOs/Exams/StartExamResponseDto.cs
public record StartExamResponseDto(
    Guid AttemptId,
    Guid ExamId,
    string TitleAr,
    string TitleEn,
    int DurationMinutes,
    DateTime StartedAt,
    DateTime ExpiresAt,
    IEnumerable<QuestionDto> Questions   // Choices returned without IsCorrect
);

// DTOs/Exams/ExamAnswerDto.cs
public record ExamAnswerDto(
    Guid QuestionId,
    Guid? SelectedChoiceId,
    string? OpenAnswer
);

// DTOs/Exams/SubmitExamDto.cs
public record SubmitExamDto(
    [Required] Guid AttemptId,
    [Required] IEnumerable<ExamAnswerDto> Answers
);

// DTOs/Exams/ExamAttemptDto.cs
public record ExamAttemptDto(
    Guid Id,
    Guid ExamId,
    string ExamTitle,
    int Score,
    bool IsPassed,
    DateTime StartedAt,
    DateTime? SubmittedAt,
    IEnumerable<ExamAnswerResultDto>? Answers   // Populated on review
);

// DTOs/Exams/ExamAnswerResultDto.cs
public record ExamAnswerResultDto(
    Guid QuestionId,
    string QuestionText,
    Guid? SelectedChoiceId,
    string? OpenAnswer,
    bool? IsCorrect,
    int? ManualGrade
);

// DTOs/Exams/GradeOpenEndedDto.cs
public record GradeOpenEndedDto(
    [Required] Guid AnswerId,
    [Required][Range(0, 100)] int Grade
);
```

---

### 2.8 Enrollment DTOs

```csharp
// DTOs/Enrollments/EnrollRequestDto.cs
public record EnrollRequestDto(
    [Required] Guid CourseId,
    string? VoucherCode,
    string? PaymentIntentId   // Stripe PaymentIntent for paid courses
);

// DTOs/Enrollments/EnrollmentDto.cs
public record EnrollmentDto(
    Guid Id,
    Guid CourseId,
    string CourseTitleAr,
    string CourseTitleEn,
    UserSummaryDto Student,
    EnrollmentStatus Status,
    decimal PaidAmount,
    string? VoucherCode,
    DateTime EnrolledAt,
    DateTime? CompletedAt
);
```

---

### 2.9 Payment DTOs

```csharp
// DTOs/Payments/CreatePaymentIntentDto.cs
public record CreatePaymentIntentDto(
    [Required] Guid CourseId,
    string? VoucherCode
);

// DTOs/Payments/ConfirmPaymentDto.cs
public record ConfirmPaymentDto(
    [Required] string PaymentIntentId
);

// DTOs/Payments/PaymentIntentResponseDto.cs
public record PaymentIntentResponseDto(
    string PaymentIntentId,
    string ClientSecret,
    decimal Amount,
    decimal OriginalPrice,
    decimal? DiscountAmount,
    string? VoucherCode,
    string Currency
);

// DTOs/Payments/ApplyVoucherDto.cs
public record ApplyVoucherDto(
    [Required] Guid CourseId,
    [Required] string VoucherCode
);
```

---

### 2.10 Voucher DTOs

```csharp
// DTOs/Vouchers/CreateVoucherDto.cs
public record CreateVoucherDto(
    [Required][MaxLength(50)] string Code,
    [Required] Guid CourseId,
    [Range(0, 100)] decimal DiscountPercent,
    decimal? DiscountAmount,
    [Range(1, 10000)] int MaxUses,
    [Required] DateTime ExpiresAt
);

// DTOs/Vouchers/UpdateVoucherDto.cs
public record UpdateVoucherDto(
    decimal? DiscountPercent,
    decimal? DiscountAmount,
    int? MaxUses,
    DateTime? ExpiresAt,
    bool? IsActive
);

// DTOs/Vouchers/VoucherDto.cs
public record VoucherDto(
    Guid Id,
    string Code,
    Guid CourseId,
    string CourseTitleEn,
    decimal DiscountPercent,
    decimal? DiscountAmount,
    int MaxUses,
    int UsedCount,
    DateTime ExpiresAt,
    bool IsActive,
    DateTime CreatedAt
);
```

---

### 2.11 Notification DTOs

```csharp
// DTOs/Notifications/NotificationDto.cs
public record NotificationDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string BodyAr,
    string BodyEn,
    NotificationType Type,
    bool IsRead,
    string? ActionUrl,
    Guid? CourseId,
    Guid? ReferenceId,
    DateTime CreatedAt
);

// DTOs/Notifications/MarkReadDto.cs
public record MarkReadDto(
    IEnumerable<Guid> NotificationIds
);
```

---

## 3. REPOSITORY INTERFACES

### 3.1 Generic Repository

```csharp
// Repositories/Interfaces/IRepository.cs
using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);

    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default);

    Task<T?> FindOneAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default);

    Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default);

    Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken ct = default);

    Task AddAsync(T entity, CancellationToken ct = default);

    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);

    void Update(T entity);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);
}
```

---

### 3.2 Specific Repository Interfaces

```csharp
// Repositories/Interfaces/ICourseRepository.cs
public interface ICourseRepository : IRepository<Course>
{
    Task<Course?> GetCourseWithDetailsAsync(Guid courseId, CancellationToken ct = default);

    Task<PagedResult<Course>> GetPagedCoursesAsync(CourseFilterDto filter, CancellationToken ct = default);

    Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId, CancellationToken ct = default);

    Task<IEnumerable<Course>> GetPublishedCoursesAsync(CancellationToken ct = default);

    Task<bool> IsInstructorOfCourseAsync(Guid instructorId, Guid courseId, CancellationToken ct = default);

    Task<Course?> GetCourseWithMaterialsAsync(Guid courseId, CancellationToken ct = default);
}

// Repositories/Interfaces/IEnrollmentRepository.cs
public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct = default);

    Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(Guid studentId, CancellationToken ct = default);

    Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId, CancellationToken ct = default);

    Task<int> GetEnrollmentCountByCourseAsync(Guid courseId, CancellationToken ct = default);
}

// Repositories/Interfaces/IUserRepository.cs
public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default);

    Task<PagedResult<ApplicationUser>> GetPagedUsersAsync(PagedRequest request, CancellationToken ct = default);

    Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(UserRole role, CancellationToken ct = default);

    Task<ApplicationUser?> GetUserWithEnrollmentsAsync(Guid userId, CancellationToken ct = default);
}

// Repositories/Interfaces/IPaymentRepository.cs
// (Stores payment intents/records for audit)
public interface IPaymentRepository : IRepository<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetPaidEnrollmentsAsync(Guid courseId, CancellationToken ct = default);

    Task<decimal> GetTotalRevenueForCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<decimal> GetTotalRevenueForInstructorAsync(Guid instructorId, CancellationToken ct = default);
}

// Repositories/Interfaces/IAssignmentRepository.cs
public interface IAssignmentRepository : IRepository<Assignment>
{
    Task<Assignment?> GetAssignmentWithSubmissionsAsync(Guid assignmentId, CancellationToken ct = default);

    Task<IEnumerable<Assignment>> GetAssignmentsByCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<AssignmentSubmission?> GetSubmissionByStudentAsync(Guid assignmentId, Guid studentId, CancellationToken ct = default);

    Task<IEnumerable<AssignmentSubmission>> GetSubmissionsByAssignmentAsync(Guid assignmentId, CancellationToken ct = default);

    Task<IEnumerable<AssignmentSubmission>> GetUngradedSubmissionsAsync(Guid assignmentId, CancellationToken ct = default);
}

// Repositories/Interfaces/IExamRepository.cs
public interface IExamRepository : IRepository<Exam>
{
    Task<Exam?> GetExamWithQuestionsAsync(Guid examId, CancellationToken ct = default);

    Task<IEnumerable<Exam>> GetExamsByCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<ExamAttempt?> GetAttemptWithAnswersAsync(Guid attemptId, CancellationToken ct = default);

    Task<IEnumerable<ExamAttempt>> GetAttemptsByStudentAsync(Guid studentId, Guid examId, CancellationToken ct = default);

    Task<int> GetAttemptCountAsync(Guid studentId, Guid examId, CancellationToken ct = default);

    Task<ExamAttempt?> GetActiveAttemptAsync(Guid studentId, Guid examId, CancellationToken ct = default);

    Task<IEnumerable<ExamAnswer>> GetOpenEndedUngradedAnswersAsync(Guid examId, CancellationToken ct = default);
}
```

---

## 4. UNIT OF WORK INTERFACE

```csharp
// Repositories/IUnitOfWork.cs
public interface IUnitOfWork : IDisposable
{
    // Repositories
    ICourseRepository Courses { get; }
    IEnrollmentRepository Enrollments { get; }
    IUserRepository Users { get; }
    IAssignmentRepository Assignments { get; }
    IExamRepository Exams { get; }

    // Generic access for entities without dedicated repositories
    IRepository<Material> Materials { get; }
    IRepository<Notification> Notifications { get; }
    IRepository<Voucher> Vouchers { get; }
    IRepository<ExamAttempt> ExamAttempts { get; }
    IRepository<ExamAnswer> ExamAnswers { get; }
    IRepository<Question> Questions { get; }
    IRepository<QuestionChoice> QuestionChoices { get; }
    IRepository<AssignmentSubmission> AssignmentSubmissions { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
```

---

## 5. SERVICE INTERFACES

### 5.1 IAuthService  ✅ IMPLEMENTED

```csharp
// Application/Services/Interfaces/IAuthService.cs
public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto, CancellationToken ct = default);

    Task<AuthResponseDto> VerifyEmailAsync(VerifyOtpDto dto, CancellationToken ct = default);

    Task<string> ResendEmailOtpAsync(ResendOtpDto dto, CancellationToken ct = default);

    Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default);

    Task<string> ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken ct = default);

    Task<AuthResponseDto> VerifyResetOtpAsync(VerifyOtpDto dto, CancellationToken ct = default);

    Task<AuthResponseDto> ResetPasswordAsync(ResetForgetPasswordDto dto, CancellationToken ct = default);

    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto, CancellationToken ct = default);

    Task LogoutAsync(RefreshTokenDto dto, CancellationToken ct = default);

    Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken ct = default);

    Task<AuthResponseDto> HandleOAuthLoginAsync(string email, CancellationToken ct = default);

    Task AssignRoleAsync(AssignRoleDto dto, CancellationToken ct = default);
}
```

---

### 5.2 IUserService

```csharp
// Application/Services/Interfaces/IUserService.cs
public interface IUserService
{
    Task<UserProfileDto> GetProfileAsync(Guid userId, CancellationToken ct = default);

    Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto dto, CancellationToken ct = default);

    Task<UserProfileDto> UpdateAvatarAsync(Guid userId, UpdateAvatarDto dto, CancellationToken ct = default);

    Task<PagedResult<UserSummaryDto>> GetAllUsersAsync(PagedRequest request, CancellationToken ct = default);

    Task<UserProfileDto> GetUserByIdAsync(Guid userId, CancellationToken ct = default);

    Task DeactivateUserAsync(Guid userId, CancellationToken ct = default);

    Task ActivateUserAsync(Guid userId, CancellationToken ct = default);

    Task<IEnumerable<UserSummaryDto>> GetInstructorsAsync(CancellationToken ct = default);
}
```

---

### 5.3 ICourseService

```csharp
// Application/Services/Interfaces/ICourseService.cs
public interface ICourseService
{
    Task<CourseDto> GetCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<PagedResult<CourseSummaryDto>> GetCoursesAsync(CourseFilterDto filter, CancellationToken ct = default);

    Task<IEnumerable<CourseSummaryDto>> GetMyCourseAsInstructorAsync(Guid instructorId, CancellationToken ct = default);

    Task<IEnumerable<CourseSummaryDto>> GetEnrolledCoursesAsync(Guid studentId, CancellationToken ct = default);

    Task<CourseDto> CreateCourseAsync(Guid instructorId, CreateCourseDto dto, CancellationToken ct = default);

    Task<CourseDto> UpdateCourseAsync(Guid courseId, Guid requesterId, UpdateCourseDto dto, CancellationToken ct = default);

    Task PublishCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task ArchiveCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task DeleteCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);
}
```

---

### 5.4 IMaterialService

```csharp
// Application/Services/Interfaces/IMaterialService.cs
public interface IMaterialService
{
    Task<IEnumerable<MaterialDto>> GetCourseMaterialsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task<MaterialDto> GetMaterialAsync(Guid materialId, Guid requesterId, CancellationToken ct = default);

    Task<MaterialDto> CreateMaterialAsync(Guid courseId, Guid instructorId, CreateMaterialDto dto, CancellationToken ct = default);

    Task<MaterialDto> UpdateMaterialAsync(Guid materialId, Guid instructorId, UpdateMaterialDto dto, CancellationToken ct = default);

    Task DeleteMaterialAsync(Guid materialId, Guid instructorId, CancellationToken ct = default);

    Task PublishMaterialAsync(Guid materialId, Guid instructorId, CancellationToken ct = default);

    Task ReorderMaterialsAsync(Guid courseId, Guid instructorId, IEnumerable<Guid> orderedIds, CancellationToken ct = default);
}
```

---

### 5.5 IAssignmentService

```csharp
// Application/Services/Interfaces/IAssignmentService.cs
public interface IAssignmentService
{
    Task<IEnumerable<AssignmentDto>> GetCourseAssignmentsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task<AssignmentDto> GetAssignmentAsync(Guid assignmentId, Guid requesterId, CancellationToken ct = default);

    Task<AssignmentDto> CreateAssignmentAsync(Guid courseId, Guid instructorId, CreateAssignmentDto dto, CancellationToken ct = default);

    Task<AssignmentDto> UpdateAssignmentAsync(Guid assignmentId, Guid instructorId, UpdateAssignmentDto dto, CancellationToken ct = default);

    Task DeleteAssignmentAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default);

    Task PublishAssignmentAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default);

    // Student actions
    Task<AssignmentSubmissionDto> SubmitAssignmentAsync(Guid assignmentId, Guid studentId, SubmitAssignmentDto dto, CancellationToken ct = default);

    Task<AssignmentSubmissionDto> GetMySubmissionAsync(Guid assignmentId, Guid studentId, CancellationToken ct = default);

    // Instructor actions
    Task<IEnumerable<AssignmentSubmissionDto>> GetSubmissionsAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default);

    Task<AssignmentSubmissionDto> GradeSubmissionAsync(Guid submissionId, Guid instructorId, GradeSubmissionDto dto, CancellationToken ct = default);
}
```

---

### 5.6 IExamService

```csharp
// Application/Services/Interfaces/IExamService.cs
public interface IExamService
{
    Task<IEnumerable<ExamDto>> GetCourseExamsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task<ExamDto> GetExamAsync(Guid examId, Guid requesterId, CancellationToken ct = default);

    Task<ExamDto> CreateExamAsync(Guid courseId, Guid instructorId, CreateExamDto dto, CancellationToken ct = default);

    Task<ExamDto> UpdateExamAsync(Guid examId, Guid instructorId, UpdateExamDto dto, CancellationToken ct = default);

    Task DeleteExamAsync(Guid examId, Guid instructorId, CancellationToken ct = default);

    Task PublishExamAsync(Guid examId, Guid instructorId, CancellationToken ct = default);

    // Questions
    Task<QuestionDto> AddQuestionAsync(Guid examId, Guid instructorId, CreateQuestionDto dto, CancellationToken ct = default);

    Task<QuestionDto> UpdateQuestionAsync(Guid questionId, Guid instructorId, UpdateQuestionDto dto, CancellationToken ct = default);

    Task DeleteQuestionAsync(Guid questionId, Guid instructorId, CancellationToken ct = default);

    // Student exam flow
    Task<StartExamResponseDto> StartExamAsync(Guid examId, Guid studentId, CancellationToken ct = default);

    Task<ExamAttemptDto> SubmitExamAsync(SubmitExamDto dto, Guid studentId, CancellationToken ct = default);

    Task<ExamAttemptDto> GetAttemptResultAsync(Guid attemptId, Guid requesterId, CancellationToken ct = default);

    Task<IEnumerable<ExamAttemptDto>> GetMyAttemptsAsync(Guid examId, Guid studentId, CancellationToken ct = default);

    // Instructor grading (open-ended)
    Task<IEnumerable<ExamAttemptDto>> GetAllAttemptsAsync(Guid examId, Guid instructorId, CancellationToken ct = default);

    Task GradeOpenEndedAnswerAsync(Guid examId, Guid instructorId, GradeOpenEndedDto dto, CancellationToken ct = default);
}
```

---

### 5.7 IEnrollmentService

```csharp
// Application/Services/Interfaces/IEnrollmentService.cs
public interface IEnrollmentService
{
    Task<EnrollmentDto> EnrollAsync(Guid studentId, EnrollRequestDto dto, CancellationToken ct = default);

    Task<EnrollmentDto> GetEnrollmentAsync(Guid enrollmentId, Guid requesterId, CancellationToken ct = default);

    Task<IEnumerable<EnrollmentDto>> GetMyEnrollmentsAsync(Guid studentId, CancellationToken ct = default);

    Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(Guid courseId, Guid instructorId, CancellationToken ct = default);

    Task CompleteEnrollmentAsync(Guid enrollmentId, Guid studentId, CancellationToken ct = default);

    Task SuspendEnrollmentAsync(Guid enrollmentId, Guid adminId, CancellationToken ct = default);

    Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId, CancellationToken ct = default);
}
```

---

### 5.8 IPaymentService

```csharp
// Application/Services/Interfaces/IPaymentService.cs
public interface IPaymentService
{
    Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(Guid studentId, CreatePaymentIntentDto dto, CancellationToken ct = default);

    Task<EnrollmentDto> ConfirmPaymentAndEnrollAsync(Guid studentId, ConfirmPaymentDto dto, CancellationToken ct = default);

    Task HandleStripeWebhookAsync(string payload, string stripeSignature, CancellationToken ct = default);

    Task<PaymentIntentResponseDto> ApplyVoucherToIntentAsync(Guid studentId, ApplyVoucherDto dto, CancellationToken ct = default);

    Task<decimal> GetCourseRevenueAsync(Guid courseId, Guid instructorId, CancellationToken ct = default);
}
```

---

### 5.9 IVoucherService

```csharp
// Application/Services/Interfaces/IVoucherService.cs
public interface IVoucherService
{
    Task<VoucherDto> CreateVoucherAsync(Guid instructorId, CreateVoucherDto dto, CancellationToken ct = default);

    Task<VoucherDto> UpdateVoucherAsync(Guid voucherId, Guid instructorId, UpdateVoucherDto dto, CancellationToken ct = default);

    Task DeleteVoucherAsync(Guid voucherId, Guid instructorId, CancellationToken ct = default);

    Task<IEnumerable<VoucherDto>> GetCourseVouchersAsync(Guid courseId, Guid instructorId, CancellationToken ct = default);

    Task<VoucherDto> ValidateVoucherAsync(string code, Guid courseId, CancellationToken ct = default);

    Task<decimal> CalculateDiscountAsync(string code, Guid courseId, CancellationToken ct = default);
}
```

---

### 5.10 INotificationService

```csharp
// Application/Services/Interfaces/INotificationService.cs
public interface INotificationService
{
    Task<PagedResult<NotificationDto>> GetMyNotificationsAsync(Guid userId, PagedRequest request, CancellationToken ct = default);

    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);

    Task MarkAsReadAsync(Guid userId, MarkReadDto dto, CancellationToken ct = default);

    Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default);

    Task DeleteNotificationAsync(Guid notificationId, Guid userId, CancellationToken ct = default);

    // Internal — called by other services to dispatch notifications
    Task NotifyNewMaterialAsync(Guid courseId, Guid materialId, CancellationToken ct = default);

    Task NotifyNewAssignmentAsync(Guid courseId, Guid assignmentId, CancellationToken ct = default);

    Task NotifyNewExamAsync(Guid courseId, Guid examId, CancellationToken ct = default);

    Task NotifyAssignmentGradedAsync(Guid studentId, Guid submissionId, CancellationToken ct = default);

    Task NotifyCourseUpdateAsync(Guid courseId, string messageAr, string messageEn, CancellationToken ct = default);

    Task SendGeneralNotificationAsync(Guid userId, string titleAr, string titleEn, string bodyAr, string bodyEn, CancellationToken ct = default);
}
```

---

## 6. CONTROLLERS

> All controllers are under `/api/v1/` and inject the corresponding **service interface** only.
> No direct repository or DbContext access.

---

### 6.1 AuthController  ✅ IMPLEMENTED

```csharp
// Controllers/v1/AuthController.cs
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    // POST api/v1/auth/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct);

    // POST api/v1/auth/verify-email
    [HttpPost("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyOtpDto dto, CancellationToken ct);

    // POST api/v1/auth/resend-otp
    [HttpPost("resend-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto dto, CancellationToken ct);

    // POST api/v1/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct);

    // POST api/v1/auth/forgot-password
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto, CancellationToken ct);

    // POST api/v1/auth/verify-reset
    [HttpPost("verify-reset")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyReset([FromBody] VerifyOtpDto dto, CancellationToken ct);

    // POST api/v1/auth/reset-password
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetForgetPasswordDto dto, CancellationToken ct);

    // POST api/v1/auth/refresh-token
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto, CancellationToken ct);

    // POST api/v1/auth/logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto, CancellationToken ct);

    // POST api/v1/auth/change-password
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken ct);

    // POST api/v1/auth/assign-role
    [HttpPost("assign-role")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto, CancellationToken ct);

    // GET api/v1/auth/login/google
    [HttpGet("login/google")]
    [AllowAnonymous]
    public IActionResult GoogleLogin([FromQuery] string? returnUrl);

    // GET api/v1/auth/login/facebook
    [HttpGet("login/facebook")]
    [AllowAnonymous]
    public IActionResult FacebookLogin([FromQuery] string? returnUrl);

    // GET api/v1/auth/external-callback
    [HttpGet("external-callback")]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalCallback(CancellationToken ct);
}
```

---

### 6.2 UsersController

```csharp
// Controllers/v1/UsersController.cs
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    // GET api/v1/users/me
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct);

    // PUT api/v1/users/me
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto, CancellationToken ct);

    // PATCH api/v1/users/me/avatar
    [HttpPatch("me/avatar")]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarDto dto, CancellationToken ct);

    // GET api/v1/users  [Admin]
    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllUsers([FromQuery] PagedRequest request, CancellationToken ct);

    // GET api/v1/users/{id}  [Admin]
    [HttpGet("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct);

    // PATCH api/v1/users/{id}/deactivate  [Admin]
    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeactivateUser(Guid id, CancellationToken ct);

    // PATCH api/v1/users/{id}/activate  [Admin]
    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ActivateUser(Guid id, CancellationToken ct);

    // GET api/v1/users/instructors  [Admin/Public]
    [HttpGet("instructors")]
    [AllowAnonymous]
    public async Task<IActionResult> GetInstructors(CancellationToken ct);
}
```

---

### 6.3 CoursesController

```csharp
// Controllers/v1/CoursesController.cs
[ApiController]
[Route("api/v1/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService) => _courseService = courseService;

    // GET api/v1/courses
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourses([FromQuery] CourseFilterDto filter, CancellationToken ct);

    // GET api/v1/courses/{id}
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourse(Guid id, CancellationToken ct);

    // GET api/v1/courses/my  [Instructor]
    [HttpGet("my")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> GetMyCourses(CancellationToken ct);

    // GET api/v1/courses/enrolled  [Student]
    [HttpGet("enrolled")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> GetEnrolledCourses(CancellationToken ct);

    // POST api/v1/courses  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto, CancellationToken ct);

    // PUT api/v1/courses/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDto dto, CancellationToken ct);

    // PATCH api/v1/courses/{id}/publish  [Instructor]
    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> PublishCourse(Guid id, CancellationToken ct);

    // PATCH api/v1/courses/{id}/archive  [Instructor/Admin]
    [HttpPatch("{id:guid}/archive")]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    public async Task<IActionResult> ArchiveCourse(Guid id, CancellationToken ct);

    // DELETE api/v1/courses/{id}  [Instructor/Admin]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken ct);
}
```

---

### 6.4 MaterialsController

```csharp
// Controllers/v1/MaterialsController.cs
[ApiController]
[Route("api/v1/courses/{courseId:guid}/materials")]
[Authorize]
public class MaterialsController : ControllerBase
{
    private readonly IMaterialService _materialService;

    public MaterialsController(IMaterialService materialService) => _materialService = materialService;

    // GET api/v1/courses/{courseId}/materials
    [HttpGet]
    public async Task<IActionResult> GetMaterials(Guid courseId, CancellationToken ct);

    // GET api/v1/courses/{courseId}/materials/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMaterial(Guid courseId, Guid id, CancellationToken ct);

    // POST api/v1/courses/{courseId}/materials  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> CreateMaterial(Guid courseId, [FromBody] CreateMaterialDto dto, CancellationToken ct);

    // PUT api/v1/courses/{courseId}/materials/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> UpdateMaterial(Guid courseId, Guid id, [FromBody] UpdateMaterialDto dto, CancellationToken ct);

    // PATCH api/v1/courses/{courseId}/materials/{id}/publish  [Instructor]
    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> PublishMaterial(Guid courseId, Guid id, CancellationToken ct);

    // PATCH api/v1/courses/{courseId}/materials/reorder  [Instructor]
    [HttpPatch("reorder")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> ReorderMaterials(Guid courseId, [FromBody] IEnumerable<Guid> orderedIds, CancellationToken ct);

    // DELETE api/v1/courses/{courseId}/materials/{id}  [Instructor]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> DeleteMaterial(Guid courseId, Guid id, CancellationToken ct);
}
```

---

### 6.5 AssignmentsController

```csharp
// Controllers/v1/AssignmentsController.cs
[ApiController]
[Route("api/v1/courses/{courseId:guid}/assignments")]
[Authorize]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService) => _assignmentService = assignmentService;

    // GET api/v1/courses/{courseId}/assignments
    [HttpGet]
    public async Task<IActionResult> GetAssignments(Guid courseId, CancellationToken ct);

    // GET api/v1/courses/{courseId}/assignments/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAssignment(Guid courseId, Guid id, CancellationToken ct);

    // POST api/v1/courses/{courseId}/assignments  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> CreateAssignment(Guid courseId, [FromBody] CreateAssignmentDto dto, CancellationToken ct);

    // PUT api/v1/courses/{courseId}/assignments/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> UpdateAssignment(Guid courseId, Guid id, [FromBody] UpdateAssignmentDto dto, CancellationToken ct);

    // PATCH api/v1/courses/{courseId}/assignments/{id}/publish  [Instructor]
    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> PublishAssignment(Guid courseId, Guid id, CancellationToken ct);

    // DELETE api/v1/courses/{courseId}/assignments/{id}  [Instructor]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> DeleteAssignment(Guid courseId, Guid id, CancellationToken ct);

    // POST api/v1/courses/{courseId}/assignments/{id}/submit  [Student]
    [HttpPost("{id:guid}/submit")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> SubmitAssignment(Guid courseId, Guid id, [FromBody] SubmitAssignmentDto dto, CancellationToken ct);

    // GET api/v1/courses/{courseId}/assignments/{id}/my-submission  [Student]
    [HttpGet("{id:guid}/my-submission")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> GetMySubmission(Guid courseId, Guid id, CancellationToken ct);

    // GET api/v1/courses/{courseId}/assignments/{id}/submissions  [Instructor]
    [HttpGet("{id:guid}/submissions")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> GetSubmissions(Guid courseId, Guid id, CancellationToken ct);

    // PATCH api/v1/courses/{courseId}/assignments/submissions/{submissionId}/grade  [Instructor]
    [HttpPatch("submissions/{submissionId:guid}/grade")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> GradeSubmission(Guid courseId, Guid submissionId, [FromBody] GradeSubmissionDto dto, CancellationToken ct);
}
```

---

### 6.6 ExamsController

```csharp
// Controllers/v1/ExamsController.cs
[ApiController]
[Route("api/v1/courses/{courseId:guid}/exams")]
[Authorize]
public class ExamsController : ControllerBase
{
    private readonly IExamService _examService;

    public ExamsController(IExamService examService) => _examService = examService;

    // GET api/v1/courses/{courseId}/exams
    [HttpGet]
    public async Task<IActionResult> GetExams(Guid courseId, CancellationToken ct);

    // GET api/v1/courses/{courseId}/exams/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetExam(Guid courseId, Guid id, CancellationToken ct);

    // POST api/v1/courses/{courseId}/exams  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> CreateExam(Guid courseId, [FromBody] CreateExamDto dto, CancellationToken ct);

    // PUT api/v1/courses/{courseId}/exams/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> UpdateExam(Guid courseId, Guid id, [FromBody] UpdateExamDto dto, CancellationToken ct);

    // PATCH api/v1/courses/{courseId}/exams/{id}/publish  [Instructor]
    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> PublishExam(Guid courseId, Guid id, CancellationToken ct);

    // DELETE api/v1/courses/{courseId}/exams/{id}  [Instructor]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> DeleteExam(Guid courseId, Guid id, CancellationToken ct);

    // POST api/v1/courses/{courseId}/exams/{id}/questions  [Instructor]
    [HttpPost("{id:guid}/questions")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> AddQuestion(Guid courseId, Guid id, [FromBody] CreateQuestionDto dto, CancellationToken ct);

    // PUT api/v1/courses/{courseId}/exams/{id}/questions/{questionId}  [Instructor]
    [HttpPut("{id:guid}/questions/{questionId:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> UpdateQuestion(Guid courseId, Guid id, Guid questionId, [FromBody] UpdateQuestionDto dto, CancellationToken ct);

    // DELETE api/v1/courses/{courseId}/exams/{id}/questions/{questionId}  [Instructor]
    [HttpDelete("{id:guid}/questions/{questionId:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> DeleteQuestion(Guid courseId, Guid id, Guid questionId, CancellationToken ct);

    // POST api/v1/courses/{courseId}/exams/{id}/start  [Student]
    [HttpPost("{id:guid}/start")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> StartExam(Guid courseId, Guid id, CancellationToken ct);

    // POST api/v1/courses/{courseId}/exams/{id}/submit  [Student]
    [HttpPost("{id:guid}/submit")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> SubmitExam(Guid courseId, Guid id, [FromBody] SubmitExamDto dto, CancellationToken ct);

    // GET api/v1/courses/{courseId}/exams/{id}/attempts/my  [Student]
    [HttpGet("{id:guid}/attempts/my")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> GetMyAttempts(Guid courseId, Guid id, CancellationToken ct);

    // GET api/v1/courses/{courseId}/exams/{id}/attempts  [Instructor]
    [HttpGet("{id:guid}/attempts")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> GetAllAttempts(Guid courseId, Guid id, CancellationToken ct);

    // GET api/v1/courses/{courseId}/exams/attempts/{attemptId}  [Student/Instructor]
    [HttpGet("attempts/{attemptId:guid}")]
    public async Task<IActionResult> GetAttemptResult(Guid courseId, Guid attemptId, CancellationToken ct);

    // PATCH api/v1/courses/{courseId}/exams/{id}/grade-open  [Instructor]
    [HttpPatch("{id:guid}/grade-open")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> GradeOpenEndedAnswer(Guid courseId, Guid id, [FromBody] GradeOpenEndedDto dto, CancellationToken ct);
}
```

---

### 6.7 EnrollmentsController

```csharp
// Controllers/v1/EnrollmentsController.cs
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService) => _enrollmentService = enrollmentService;

    // POST api/v1/enrollments  [Student]
    [HttpPost]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> Enroll([FromBody] EnrollRequestDto dto, CancellationToken ct);

    // GET api/v1/enrollments/my  [Student]
    [HttpGet("my")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> GetMyEnrollments(CancellationToken ct);

    // GET api/v1/enrollments/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEnrollment(Guid id, CancellationToken ct);

    // GET api/v1/enrollments/course/{courseId}  [Instructor/Admin]
    [HttpGet("course/{courseId:guid}")]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    public async Task<IActionResult> GetCourseEnrollments(Guid courseId, CancellationToken ct);

    // PATCH api/v1/enrollments/{id}/complete  [Student]
    [HttpPatch("{id:guid}/complete")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> CompleteEnrollment(Guid id, CancellationToken ct);

    // PATCH api/v1/enrollments/{id}/suspend  [Admin]
    [HttpPatch("{id:guid}/suspend")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SuspendEnrollment(Guid id, CancellationToken ct);
}
```

---

### 6.8 PaymentsController

```csharp
// Controllers/v1/PaymentsController.cs
[ApiController]
[Route("api/v1/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService) => _paymentService = paymentService;

    // POST api/v1/payments/intent  [Student]
    [HttpPost("intent")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentDto dto, CancellationToken ct);

    // POST api/v1/payments/confirm  [Student]
    [HttpPost("confirm")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDto dto, CancellationToken ct);

    // POST api/v1/payments/apply-voucher  [Student]
    [HttpPost("apply-voucher")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> ApplyVoucher([FromBody] ApplyVoucherDto dto, CancellationToken ct);

    // POST api/v1/payments/webhook  [Stripe — no auth]
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook(CancellationToken ct);

    // GET api/v1/payments/courses/{courseId}/revenue  [Instructor]
    [HttpGet("courses/{courseId:guid}/revenue")]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    public async Task<IActionResult> GetCourseRevenue(Guid courseId, CancellationToken ct);
}
```

---

### 6.9 VouchersController

```csharp
// Controllers/v1/VouchersController.cs
[ApiController]
[Route("api/v1/courses/{courseId:guid}/vouchers")]
[Authorize]
public class VouchersController : ControllerBase
{
    private readonly IVoucherService _voucherService;

    public VouchersController(IVoucherService voucherService) => _voucherService = voucherService;

    // GET api/v1/courses/{courseId}/vouchers  [Instructor]
    [HttpGet]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    public async Task<IActionResult> GetVouchers(Guid courseId, CancellationToken ct);

    // POST api/v1/courses/{courseId}/vouchers  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> CreateVoucher(Guid courseId, [FromBody] CreateVoucherDto dto, CancellationToken ct);

    // PUT api/v1/courses/{courseId}/vouchers/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> UpdateVoucher(Guid courseId, Guid id, [FromBody] UpdateVoucherDto dto, CancellationToken ct);

    // DELETE api/v1/courses/{courseId}/vouchers/{id}  [Instructor]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    public async Task<IActionResult> DeleteVoucher(Guid courseId, Guid id, CancellationToken ct);

    // POST api/v1/courses/{courseId}/vouchers/validate  [Student]
    [HttpPost("validate")]
    [Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> ValidateVoucher(Guid courseId, [FromBody] string code, CancellationToken ct);
}
```

---

### 6.10 NotificationsController

```csharp
// Controllers/v1/NotificationsController.cs
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService) => _notificationService = notificationService;

    // GET api/v1/notifications
    [HttpGet]
    public async Task<IActionResult> GetMyNotifications([FromQuery] PagedRequest request, CancellationToken ct);

    // GET api/v1/notifications/unread-count
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken ct);

    // PATCH api/v1/notifications/mark-read
    [HttpPatch("mark-read")]
    public async Task<IActionResult> MarkAsRead([FromBody] MarkReadDto dto, CancellationToken ct);

    // PATCH api/v1/notifications/mark-all-read
    [HttpPatch("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken ct);

    // DELETE api/v1/notifications/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteNotification(Guid id, CancellationToken ct);
}
```

---

## 7. CONSTANTS & SECURITY

```csharp
// Common/Constants/Roles.cs
public static class Roles
{
    public const string Admin      = "Admin";
    public const string Instructor = "Instructor";
    public const string Student    = "Student";
}

// Common/Constants/Policies.cs
public static class Policies
{
    public const string MustBeOwnerOrAdmin = "MustBeOwnerOrAdmin";
    public const string MustBeEnrolled     = "MustBeEnrolled";
}
```

---

## 8. STRIPE INTEGRATION SKETCH

```csharp
// Common/StripeSettings.cs
public class StripeSettings
{
    public string SecretKey     { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;
    public string Currency      { get; set; } = "usd";
}

// Infrastructure/Services/StripeService.cs (interface only)
public interface IStripeService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency, Dictionary<string, string> metadata, CancellationToken ct = default);

    Task<bool> ConfirmPaymentIntentAsync(string paymentIntentId, CancellationToken ct = default);

    Task<string> GetPaymentIntentStatusAsync(string paymentIntentId, CancellationToken ct = default);

    bool ValidateWebhookSignature(string payload, string signature, out object stripeEvent);
}
```

---

## 9. PAYMENT FLOW DIAGRAM

```
Student                 API                    Stripe
  |                      |                        |
  |-- POST /payments/    |                        |
  |   intent             |                        |
  |                      |-- CreatePaymentIntent->|
  |                      |<-- clientSecret -------|
  |<-- PaymentIntentDto  |                        |
  |    (clientSecret)    |                        |
  |                      |                        |
  |-- [Stripe.js handles payment on frontend]     |
  |                      |                        |
  |                      |<-- Webhook (paid) -----|
  |                      |    HandleWebhook        |
  |                      |    → CreateEnrollment  |
  |                      |    → SendNotification  |
  |                      |                        |
  |-- POST /payments/    |                        |
  |   confirm            |                        |
  |                      |-- VerifyStatus ------->|
  |<-- EnrollmentDto     |                        |
```

---

## 10. DEPENDENCY INJECTION REGISTRATION

```csharp
// Program.cs — Service registrations

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();          // ✅ DONE
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Infrastructure Services
builder.Services.AddScoped<TokenService>();                        // ✅ DONE
builder.Services.AddScoped<OtpService>();                          // ✅ DONE
builder.Services.AddScoped<EmailService>();                        // ✅ DONE
builder.Services.AddScoped<IStripeService, StripeService>();

// Settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
```
