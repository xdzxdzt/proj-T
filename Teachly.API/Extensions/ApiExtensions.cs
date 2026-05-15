using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Teachly.API.Endpoints;
using Teachly.Core.Enums;
using Teachly.Infrastructure;

namespace Teachly.API.Extensions
{
    public static class ApiExtensions
    {
        public static void AddMappedEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapUsersEndpoints();
            app.MapTutorTasksEndpoints();
            app.MapTutorSubjectsEndpoints();
            app.MapTutorFeedbackEndpoints();
            app.MapSolutionsEndpoints();
            app.MapReviewsEndpoints();
            app.MapLessonsEndpoints();
            app.MapReportsEndpoints();
            app.MapDictionariesEndpoints();
        }

        public static void AddApiAuthentication(this IServiceCollection services, IOptions<JwtOptions> jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["not-jwt-token"];

                            return Task.CompletedTask;
                        } 
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TutorPolicy", policy =>
                {
                    policy.RequireRole(UserRole.Tutor.ToString());
                });

                options.AddPolicy("StudentPolicy", policy =>
                {
                    policy.RequireRole(UserRole.Student.ToString());
                });
            });
        }
    }
}
