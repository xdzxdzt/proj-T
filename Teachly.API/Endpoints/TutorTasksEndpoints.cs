using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.TutorTasks;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class TutorTasksEndpoints
    {
        public static IEndpointRouteBuilder MapTutorTasksEndpoints(this IEndpointRouteBuilder app)
        {
            var tutorTask = app.MapGroup("tutor-task");

            tutorTask.MapPost("createTask", CreateTask);
            tutorTask.MapGet("lesson-package/{lessonPackageId:guid}", GetByLessonPackageId);
            tutorTask.MapPatch("closeTask", CloseTask);

            return app;
        }

        private static async Task<IResult> CreateTask(
            [FromBody] CreateTaskRequest request,
            ITutorTasksService tutorTasksService)
        {
            try
            {
                await tutorTasksService.CreateTask(
                request.TutorId,
                request.LessonPackageId,
                request.Title,
                request.Description);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetByLessonPackageId(
            Guid lessonPackageId,
            ITutorTasksService tutorTasksService)
        {
            try
            {
                var tasks = await tutorTasksService.GetByLessonPackageId(lessonPackageId);

                return Results.Ok(tasks);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> CloseTask(
            [FromBody] CloseTaskRequest request, 
            ITutorTasksService tutorTasksService)
        {
            try
            {
                await tutorTasksService.CloseTask(
                request.TutorId,
                request.TaskId);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message});
            }
        }
    }
}
