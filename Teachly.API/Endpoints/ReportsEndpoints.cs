using Teachly.API.Contracts.Reports;
using Teachly.API.Extensions;
using Teachly.Application.Interfaces.Services;
using Teachly.Application.Reports;

namespace Teachly.API.Endpoints
{
    public static class ReportsEndpoints
    {
        public static IEndpointRouteBuilder MapReportsEndpoints(this IEndpointRouteBuilder app)
        {
            var reports = app.MapGroup("reports");

            reports.MapGet("my/progress", GetMyProgressReport)
                .RequireAuthorization("StudentPolicy");

            reports.MapGet("students/{studentId:guid}/progress", GetStudentProgressReport)
                .RequireAuthorization("TutorPolicy");

            return app;
        }

        private static async Task<IResult> GetMyProgressReport(
            HttpContext context,
            IReportsService reportsService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                var report = await reportsService.GetStudentProgressReportForStudent(userId);

                return Results.Ok(ToResponse(report));
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetStudentProgressReport(
            Guid studentId,
            HttpContext context,
            IReportsService reportsService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var userId))
                {
                    return Results.Unauthorized();
                }

                var report = await reportsService.GetStudentProgressReportForTutor(userId, studentId);

                return Results.Ok(ToResponse(report));
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static StudentProgressReportResponse ToResponse(StudentProgressReport report)
        {
            return new StudentProgressReportResponse(
                report.StudentId,
                report.SubmittedSolutions,
                report.CheckedSolutions,
                report.AverageGrade);
        }
    }
}
