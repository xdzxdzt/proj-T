using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.TutorSubject;
using Teachly.API.Extensions;
using Teachly.Application.Reports;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class TutorSubjectsEndpoints
    {
        public static IEndpointRouteBuilder MapTutorSubjectsEndpoints(this IEndpointRouteBuilder app)
        {
            var tutorSubjects = app.MapGroup("tutor-subjects");
            tutorSubjects.MapPost("", AddSubjectToTutor).RequireAuthorization("TutorPolicy");
            tutorSubjects.MapGet("", GetAll);
            tutorSubjects.MapGet("tutors/{tutorId:guid}", GetByTutorId);
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

        private static async Task<IResult> GetAll(
            ITutorSubjectService tutorSubjectService)
        {
            try
            {
                var tutorSubjects = await tutorSubjectService.GetAll();

                return Results.Ok(tutorSubjects.Select(ToResponse).ToList());
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetByTutorId(
            Guid tutorId,
            ITutorSubjectService tutorSubjectService)
        {
            try
            {
                var tutorSubjects = await tutorSubjectService.GetByTutorId(tutorId);

                return Results.Ok(tutorSubjects.Select(ToResponse).ToList());
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static TutorSubjectResponse ToResponse(TutorSubjectInfo tutorSubject)
        {
            return new TutorSubjectResponse(
                tutorSubject.Id,
                tutorSubject.TutorId,
                tutorSubject.TutorUserId,
                tutorSubject.TutorUserName,
                tutorSubject.TutorFirstName,
                tutorSubject.TutorLastName,
                tutorSubject.TutorAvatarUrl,
                tutorSubject.TutorDescription,
                tutorSubject.TutorAverageRating,
                tutorSubject.TutorRatingCount,
                tutorSubject.SubjectId,
                tutorSubject.SubjectName,
                tutorSubject.PricePerLesson);
        }
    }
}
