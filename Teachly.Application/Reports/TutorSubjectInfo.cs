namespace Teachly.Application.Reports
{
    public record TutorSubjectInfo(
        Guid Id,
        Guid TutorId,
        Guid TutorUserId,
        string TutorUserName,
        string TutorFirstName,
        string TutorLastName,
        string? TutorAvatarUrl,
        string? TutorDescription,
        decimal TutorAverageRating,
        int TutorRatingCount,
        Guid SubjectId,
        string SubjectName,
        decimal PricePerLesson);
}
