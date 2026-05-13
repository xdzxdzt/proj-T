using System.ComponentModel.DataAnnotations;

namespace Teachly.API.Contracts.TutorSubject
{
    public record TutorSubjectRequest(
        [Required]
        Guid SubjectId,
        [Range(1, int.MaxValue)]
        decimal PricePerLesson
        );
}
