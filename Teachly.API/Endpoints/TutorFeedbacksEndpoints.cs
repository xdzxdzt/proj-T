using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.TutorFeedbacks;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class TutorFeedbacksEndpoints
    {
        public static IEndpointRouteBuilder MapTutorFeedbackEndpoints(
            this IEndpointRouteBuilder app)
        {
            var feedback = app.MapGroup("tutor-feedback");

            feedback.MapPost("", GiveFeedback);
            return app;
        }

        private static async Task<IResult> GiveFeedback(
            [FromBody] FeedbackRequest request,
            ITutorFeedbacksService tutorFeedbackService)
        {
            try
            {
                await tutorFeedbackService.GiveFeedback(
                    request.TutorId,
                    request.SolutionId,
                    request.Grade,
                    request.TutorComment);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {

                return Results.BadRequest(new { error = ex.Message });
            }
        }
    }
}
