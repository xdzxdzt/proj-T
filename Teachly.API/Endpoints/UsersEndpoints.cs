using Teachly.API.Contracts.Auth;
using Teachly.Application.Interfaces.Services;

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

        private static async Task<IResult> RegisterStudent(RegisterStudentRequest request, IUsersService usersService)
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

        private static async Task<IResult> RegisterTutor(RegisterTutorRequest request, IUsersService usersService)
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

        private static async Task<IResult> Login(LoginUserRequest request, IUsersService usersService, HttpContext context)
        {
            var token = await usersService.Login(request.Email, request.Password);

            context.Response.Cookies.Append("not-jwt-token", token);

            return Results.Ok(new { token });
        }
    }
}
