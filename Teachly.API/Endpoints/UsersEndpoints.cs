using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Teachly.API.Contracts.Auth;
using Teachly.API.Contracts.Profiles;
using Teachly.API.Extensions;
using Teachly.Application.Interfaces.Services;

namespace Teachly.API.EndPoints
{
    public static class UsersEndpoints
    {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            var auth = app.MapGroup("auth");

            auth.MapPost("register/student", RegisterStudent);
            auth.MapPost("register/tutor", RegisterTutor);
            auth.MapPost("login", Login);

            var profiles = app.MapGroup("profiles");

            profiles.MapGet("students/{studentId:guid}", GetStudentProfile);
            profiles.MapGet("tutors/{tutorId:guid}", GetTutorProfile);
            profiles.MapPost("users/{userId:guid}/avatar", UploadAvatar)
                .RequireAuthorization()
                .DisableAntiforgery();

            return app;
        }

        private static async Task<IResult> RegisterStudent(
            [FromBody] RegisterStudentRequest request,
            IUsersService usersService)
        {
            try
            {
                await usersService.RegisterStudent(
                    request.UserName,
                    request.FirstName,
                    request.LastName,
                    request.Age,
                    request.Email,
                    request.Password,
                    request.InstitutionId,
                    request.EducationLevel,
                    request.ParentPhone);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> RegisterTutor(
            [FromBody] RegisterTutorRequest request,
            IUsersService usersService)
        {
            try
            {
                await usersService.RegisterTutor(
                    request.UserName,
                    request.FirstName,
                    request.LastName,
                    request.Age,
                    request.Email,
                    request.Password,
                    request.Description);

                return Results.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> Login(
            [FromBody] LoginUserRequest request,
            IUsersService usersService,
            HttpContext context)
        {
            try
            {
                var token = await usersService.Login(request.Email, request.Password);

                context.Response.Cookies.Append("not-jwt-token", token);

                return Results.Ok(new { token });
            }
            catch (InvalidOperationException)
            {
                return Results.Unauthorized();
            }
        }

        private static async Task<IResult> GetStudentProfile(
            Guid studentId,
            IUsersService usersService)
        {
            try
            {
                var (student, user) = await usersService.GetStudentProfile(studentId);
                var profile = new StudentProfileResponse(
                    student.Id,
                    user.Id,
                    user.UserName,
                    user.FirstName,
                    user.LastName,
                    user.Age,
                    user.Email,
                    user.AvatarUrl,
                    student.InstitutionId,
                    student.EducationLevel,
                    student.ParentPhone);

                return Results.Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> GetTutorProfile(
            Guid tutorId,
            IUsersService usersService)
        {
            try
            {
                var (tutor, user) = await usersService.GetTutorProfile(tutorId);
                var profile = new TutorProfileResponse(
                    tutor.Id,
                    user.Id,
                    user.UserName,
                    user.FirstName,
                    user.LastName,
                    user.Age,
                    user.Email,
                    user.AvatarUrl,
                    tutor.Description,
                    tutor.AverageRating,
                    tutor.RatingCount);

                return Results.Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }

        private static async Task<IResult> UploadAvatar(
            Guid userId,
            [Required] IFormFile avatar,
            HttpContext context,
            IWebHostEnvironment environment,
            IUsersService usersService)
        {
            try
            {
                if (!context.User.TryGetUserId(out var currentUserId))
                {
                    return Results.Unauthorized();
                }

                if (currentUserId != userId)
                {
                    return Results.Forbid();
                }

                if (avatar.Length == 0)
                {
                    return Results.BadRequest(new { error = "Файл аватарки пустой" });
                }

                const long maxAvatarSize = 5 * 1024 * 1024;

                if (avatar.Length > maxAvatarSize)
                {
                    return Results.BadRequest(new { error = "Размер аватарки не должен превышать 5 МБ" });
                }

                var extension = Path.GetExtension(avatar.FileName).ToLowerInvariant();
                var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };

                if (!allowedExtensions.Contains(extension))
                {
                    return Results.BadRequest(new { error = "Допустимые форматы аватарки: jpg, jpeg, png, webp" });
                }

                var uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads", "avatars");
                Directory.CreateDirectory(uploadsDirectory);

                var fileName = $"{userId}_{Guid.NewGuid():N}{extension}";
                var filePath = Path.Combine(uploadsDirectory, fileName);

                await using (var fileStream = File.Create(filePath))
                {
                    await avatar.CopyToAsync(fileStream);
                }

                var avatarUrl = $"/uploads/avatars/{fileName}";
                await usersService.SetAvatar(userId, avatarUrl);

                return Results.Ok(new { avatarUrl });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }
    }
}
