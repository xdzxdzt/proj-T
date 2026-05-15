using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.LessonPackage;
using Teachly.API.Extensions;
using Teachly.Application.Reports;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class LessonPackagesEndpoints
    {
        public static IEndpointRouteBuilder MapLessonsEndpoints(this IEndpointRouteBuilder app)
        {
            var lessonPackage = app.MapGroup("lesson-package");
            lessonPackage.MapPost("buylesson", BuyLesson).RequireAuthorization("StudentPolicy");
            lessonPackage.MapPatch("uselesson", UseLesson).RequireAuthorization("TutorPolicy");
            lessonPackage.MapGet("my", GetMyLessonPackages).RequireAuthorization("StudentPolicy");
            lessonPackage.MapGet("tutor/my", GetMyTutorLessonPackages).RequireAuthorization("TutorPolicy");
            return app;
        }

        private static async Task<IResult> BuyLesson(
            [FromBody] BuyLessonRequest request,
            HttpContext context,
            ILessonPackagesService lessonPackageService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                await lessonPackageService.BuyPackage(
                    userId,
                    request.TutorSubjectId,
                    request.TotalLessons);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> UseLesson(
            [FromBody] UseLessonRequest request,
            HttpContext context,
            ILessonPackagesService lessonPackagesService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                await lessonPackagesService.UseLesson(
                    userId,
                    request.LessonPackageId);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetMyLessonPackages(
            HttpContext context,
            ILessonPackagesService lessonPackagesService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                var lessonPackages = await lessonPackagesService.GetForStudent(userId);

                return Results.Ok(lessonPackages.Select(ToResponse).ToList());
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetMyTutorLessonPackages(
            HttpContext context,
            ILessonPackagesService lessonPackagesService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                var lessonPackages = await lessonPackagesService.GetForTutor(userId);

                return Results.Ok(lessonPackages.Select(ToResponse).ToList());
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static LessonPackageResponse ToResponse(LessonPackageInfo lessonPackage)
        {
            return new LessonPackageResponse(
                lessonPackage.Id,
                lessonPackage.StudentId,
                lessonPackage.StudentUserId,
                lessonPackage.StudentUserName,
                lessonPackage.StudentFirstName,
                lessonPackage.StudentLastName,
                lessonPackage.TutorSubjectId,
                lessonPackage.TutorId,
                lessonPackage.TutorUserId,
                lessonPackage.TutorUserName,
                lessonPackage.TutorFirstName,
                lessonPackage.TutorLastName,
                lessonPackage.SubjectId,
                lessonPackage.SubjectName,
                lessonPackage.TotalLessons,
                lessonPackage.RemainingLessons,
                lessonPackage.PricePerLesson,
                lessonPackage.TotalPrice,
                lessonPackage.PurchasedAt,
                lessonPackage.CompletedAt);
        }
    }
}
