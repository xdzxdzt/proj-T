using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.TutorTasks;
using Teachly.API.Extensions;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class TutorTasksEndpoints
    {
        public static IEndpointRouteBuilder MapTutorTasksEndpoints(this IEndpointRouteBuilder app)
        {
            var tutorTask = app.MapGroup("tutor-task");

            tutorTask.MapPost("createTask", CreateTask).RequireAuthorization("TutorPolicy");
            tutorTask.MapGet("lesson-package/{lessonPackageId:guid}", GetByLessonPackageId).RequireAuthorization("TutorPolicy");
            tutorTask.MapGet("my", GetMyTasks).RequireAuthorization("StudentPolicy");
            tutorTask.MapPatch("closeTask", CloseTask).RequireAuthorization("TutorPolicy");

            return app;
        }

        private static async Task<IResult> CreateTask(
            [FromBody] CreateTaskRequest request,
            HttpContext context,
            ITutorTasksService tutorTasksService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                await tutorTasksService.CreateTask(
                    userId,
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
            HttpContext context,
            ITutorTasksService tutorTasksService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                var tasks = await tutorTasksService.GetByLessonPackageId(userId, lessonPackageId);

                return Results.Ok(tasks);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetMyTasks(
            HttpContext context,
            ITutorTasksService tutorTasksService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                var tasks = await tutorTasksService.GetForStudent(userId);
                var response = tasks
                    .Select(t => new StudentTaskResponse(
                        t.Id,
                        t.LessonPackageId,
                        t.Title,
                        t.Description,
                        t.CreatedAt,
                        t.ClosedAt))
                    .ToList();

                return Results.Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> CloseTask(
            [FromBody] CloseTaskRequest request,
            HttpContext context,
            ITutorTasksService tutorTasksService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                await tutorTasksService.CloseTask(
                    userId,
                    request.TaskId);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }
    }
}
