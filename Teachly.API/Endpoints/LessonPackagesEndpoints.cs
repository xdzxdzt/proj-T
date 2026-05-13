using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.LessonPackage;
using Teachly.API.Extensions;
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
    }
}
