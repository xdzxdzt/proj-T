using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.SubmitSolutions;
using Teachly.API.Extensions;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class SolutionsEndpoints
    {
        public static IEndpointRouteBuilder MapSolutionsEndpoints(this IEndpointRouteBuilder app)
        {
            var auth = app.MapGroup("solution");
            auth.MapPost("submit", SubmitSolution).RequireAuthorization("StudentPolicy");
            auth.MapGet("my/history", GetMyHistory).RequireAuthorization("StudentPolicy");
            auth.MapGet("student/{studentId:guid}", GetAllByStudentId).RequireAuthorization("TutorPolicy");
            return app;
        }

        private static async Task<IResult> SubmitSolution(
            [FromBody] SubmitSolutionRequest request,
            HttpContext context,
            ISolutionsService solutionService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                await solutionService.SubmitSolution(
                    userId,
                    request.TutorTaskId,
                    request.AnswerText);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetMyHistory(
            HttpContext context,
            ISolutionsService solutionService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                var history = await solutionService.GetHistoryForStudent(userId);
                var response = history
                    .Select(x => new StudentSolutionHistoryResponse(
                        x.Solution.Id,
                        x.TutorTask.Id,
                        x.TutorTask.LessonPackageId,
                        x.TutorTask.Title,
                        x.Solution.AnswerText,
                        x.Solution.SubmittedAt,
                        x.TutorFeedback?.Id,
                        x.TutorFeedback?.Grade,
                        x.TutorFeedback?.TutorComment,
                        x.TutorFeedback?.GivenAt))
                    .ToList();

                return Results.Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetAllByStudentId(
            Guid studentId,
            HttpContext context,
            ISolutionsService solutionService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                var solutions = await solutionService.GetAllByStudentId(userId, studentId);

                return Results.Ok(solutions);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }
    }
}
