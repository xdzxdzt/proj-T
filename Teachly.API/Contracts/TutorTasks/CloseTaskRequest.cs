using System.ComponentModel.DataAnnotations;

namespace Teachly.API.Contracts.TutorTasks
{
    public record CloseTaskRequest(
        [Required]
        Guid TutorId,
        [Required]
        Guid TaskId);
}
