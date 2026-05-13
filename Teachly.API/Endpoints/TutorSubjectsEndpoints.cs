using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.TutorSubject;
using Teachly.API.Extensions;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class TutorSubjectsEndpoints
    {
        public static IEndpointRouteBuilder MapTutorSubjectsEndpoints(this IEndpointRouteBuilder app)
        {
            var tutorSubjects = app.MapGroup("tutor-subjects");
            tutorSubjects.MapPost("", AddSubjectToTutor).RequireAuthorization("TutorPolicy");
            return app;
        }

        private static async Task<IResult> AddSubjectToTutor(
            [FromBody] TutorSubjectRequest request,
            HttpContext context,
            ITutorSubjectService tutorSubjectService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                await tutorSubjectService.AddSubjectToTutor(
                    userId,
                    request.SubjectId,
                    request.PricePerLesson);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }
    }
}
