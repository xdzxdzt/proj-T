using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.Reviews;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class ReviewsEndpoints
    {
        public static IEndpointRouteBuilder MapReviewsEndpoints(this IEndpointRouteBuilder app)
        {
            var auth = app.MapGroup("review");

            auth.MapPost("add", AddReview);

            return app;
        }

        private static async Task<IResult> AddReview(
            [FromBody] AddReviewRequest request,
            IReviewsService reviewService)
        {
            try
            {
                await reviewService.CreateReview(
                    request.TutorId,
                    request.StudentId,
                    request.ReviewText,
                    request.Rating);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            { 
                return Results.BadRequest(new { error = ex.Message });
            }
        }
    }
}
