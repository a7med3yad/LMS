namespace LMS.Domain.Common.Constants;

public static class Roles
{
    public const string Admin      = "Admin";
    public const string Instructor = "Instructor";
    public const string Student    = "Student";

    /// <summary>All roles that must exist in AspNetRoles.</summary>
    public static readonly string[] All = [Admin, Instructor, Student];
}
