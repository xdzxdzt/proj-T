using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.TutorTasks
{
    public record CreateTaskRequest(
        [Required]
        Guid TutorId,
        [Required]
        Guid LessonPackageId,
        [Required]
        [MaxLength(TutorTask.MAX_TITLE_LENGTH)]
        string Title,
        [Required]
        [MaxLength(TutorTask.MAX_DESCRIPTION_LENGTH)]
        string Description
        );
}
