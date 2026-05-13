using System.ComponentModel.DataAnnotations;

namespace Teachly.API.Contracts.LessonPackage
{
    public record BuyLessonRequest(
        [Required]
        Guid TutorSubjectId,
        [Required]
        [Range(1,int.MaxValue)]
        int TotalLessons);
}
