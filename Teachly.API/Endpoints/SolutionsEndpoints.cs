using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.SubmitSolutions;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class SolutionsEndpoints
    {
        public static IEndpointRouteBuilder MapSolutionsEndpoints(this IEndpointRouteBuilder app)
        {
            var auth = app.MapGroup("solution");
            auth.MapPost("submit", SubmitSolution);
            auth.MapGet("student/{studentId:guid}", GetAllByStudentId);
            return app;
        }
        private static async Task<IResult> SubmitSolution(
            [FromBody] SubmitSolutionRequest request,
            ISolutionsService solutionService)
        {
            try
            {
                await solutionService.SubmitSolution(
                    request.StudentId,
                    request.TutorTaskId,
                    request.AnswerText);
                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            { 
                return Results.BadRequest(new { error = ex.Message } );
            }
        }
        private static async Task<IResult> GetAllByStudentId(
            Guid studentId,
            ISolutionsService solutionService)
        {
            try
            {
                var solutions = await solutionService.GetAllByStudentId(studentId);

                return Results.Ok(solutions);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }
    }
}
