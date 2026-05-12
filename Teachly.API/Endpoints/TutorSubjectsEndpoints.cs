using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.TutorSubject;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class TutorSubjectsEndpoints
    {
        public static IEndpointRouteBuilder MapTutorSubjectsEndpoints(this IEndpointRouteBuilder app)
        {
            var tutorSubjects = app.MapGroup("tutor-subjects");
            tutorSubjects.MapPost("", AddSubjectToTutor);
            return app;
        }

        private static async Task<IResult> AddSubjectToTutor(
            [FromBody] TutorSubjectRequest request,
            ITutorSubjectService tutorSubjectService)
        {
            try
            {
                await tutorSubjectService.AddSubjectToTutor(
                request.TutorId,
                request.SubjectId,
                request.PricePerLesson);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new {error = ex.Message});
            }

        }
    }
}
