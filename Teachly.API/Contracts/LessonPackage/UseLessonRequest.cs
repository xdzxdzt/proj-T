using System.ComponentModel.DataAnnotations;

namespace Teachly.API.Contracts.LessonPackage
{
    public record UseLessonRequest(
        [Required]
        Guid LessonPackageId);
}
