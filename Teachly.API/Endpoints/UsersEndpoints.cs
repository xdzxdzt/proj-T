using Teachly.API.Contracts;
using Teachly.Application.Services;
using Teachly.Core.Enums;

namespace Teachly.API.EndPoints
{
    public static class UsersEndpoints
    {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            var users = app.MapGroup("users");

            users.MapPost("register/student", RegisterStudent);
            users.MapPost("register/tutor", RegisterTutor);
            users.MapPost("login", Login);

            return app;
        }

        private static async Task<IResult> RegisterStudent(RegisterStudentRequest request, UsersService usersService)
        {
            await usersService.Register(
                request.UserName,
                request.FirstName,
                request.LastName,
                request.Age,
                request.Email,
                request.Password,
                UserRole.Student);

            return Results.Ok();
        }

        private static async Task<IResult> RegisterTutor(RegisterTutorRequest request, UsersService usersService)
        {
            await usersService.Register(
                request.UserName,
                request.FirstName,
                request.LastName,
                request.Age,
                request.Email,
                request.Password,
                UserRole.Tutor);

            return Results.Ok();
        }

        private static async Task<IResult> Login(LoginUserRequest request, UsersService usersService, HttpContext context)
        {
            var token = await usersService.Login(request.Email, request.Password);

            context.Response.Cookies.Append("not-jwt-token", token);

            return Results.Ok(new { token });
        }
    }
}
