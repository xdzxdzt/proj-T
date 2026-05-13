using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.TutorFeedbacks;
using Teachly.API.Extensions;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class TutorFeedbacksEndpoints
    {
        public static IEndpointRouteBuilder MapTutorFeedbackEndpoints(
            this IEndpointRouteBuilder app)
        {
            var feedback = app.MapGroup("tutor-feedback");

            feedback.MapPost("", GiveFeedback).RequireAuthorization("TutorPolicy");
            return app;
        }

        private static async Task<IResult> GiveFeedback(
            [FromBody] FeedbackRequest request,
            HttpContext context,
            ITutorFeedbacksService tutorFeedbackService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                await tutorFeedbackService.GiveFeedback(
                    userId,
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
