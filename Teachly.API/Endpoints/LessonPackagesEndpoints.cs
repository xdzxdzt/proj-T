using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.LessonPackage;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.Endpoints
{
    public static class LessonPackagesEndpoints
    {
        public static IEndpointRouteBuilder MapLessonsEndpoints(this IEndpointRouteBuilder app)
        {
            var lessonPackage = app.MapGroup("lesson-package");
            lessonPackage.MapPost("buylesson", BuyLesson);
            lessonPackage.MapPatch("uselesson", UseLesson);
            return app;
        }

        private static async Task<IResult> BuyLesson(
            [FromBody] BuyLessonRequest request,
            ILessonPackagesService lessonPackageService)
        {
            try
            {
                await lessonPackageService.BuyPackage(
                    request.StudentId,
                    request.TutorSubjectId,
                    request.TotalLessons);

                return Results.Ok();
            }
            catch(InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> UseLesson(
            [FromBody] UseLessonRequest request,
            ILessonPackagesService lessonPackagesService)
        {
            try
            {
                await lessonPackagesService.UseLesson(
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
