namespace Teachly.API.Contracts.LessonPackage
{
    public record LessonPackageResponse(
        Guid Id,
        Guid StudentId,
        Guid StudentUserId,
        string StudentUserName,
        string StudentFirstName,
        string StudentLastName,
        Guid TutorSubjectId,
        Guid TutorId,
        Guid TutorUserId,
        string TutorUserName,
        string TutorFirstName,
        string TutorLastName,
        Guid SubjectId,
        string SubjectName,
        int TotalLessons,
        int RemainingLessons,
        decimal PricePerLesson,
        decimal TotalPrice,
        DateTime PurchasedAt,
        DateTime? CompletedAt);
}
