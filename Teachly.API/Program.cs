using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Auth;
using Teachly.Application.Interfaces.Repositories;
using Teachly.DataAccess;
using Teachly.DataAccess.Repositories;
using Teachly.Infrastructure;
using Microsoft.AspNetCore.CookiePolicy;
using Teachly.Application.Interfaces.Services;
using Teachly.Application.Services;
using Teachly.API.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


services.AddControllers();

services.AddEndpointsApiExplorer();

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
    ?? throw new InvalidOperationException("JwtOptions section is not configured");
services.AddApiAuthentication(Options.Create(jwtOptions));

services.AddSwaggerGen();

services.AddDbContext<TeachlyDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

services.AddScoped<IUsersRepository, UsersRepository>();
services.AddScoped<IStudentsRepository, StudentsRepository>();
services.AddScoped<ITutorsRepository, TutorsRepository>();
services.AddScoped<IInstitutionsRepository, InstitutionsRepository>();
services.AddScoped<ISubjectsRepository, SubjectsRepository>();
services.AddScoped<ITutorSubjectsRepository, TutorSubjectsRepository>();
services.AddScoped<ILessonPackagesRepository, LessonPackagesRepository>();
services.AddScoped<ITutorTasksRepository, TutorTasksRepository>();
services.AddScoped<ISolutionsRepository, SolutionsRepository>();
services.AddScoped<ITutorFeedbacksRepository, TutorFeedbacksRepository>();
services.AddScoped<IReviewsRepository, ReviewsRepository>();

services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IUsersService, UsersService>();
services.AddScoped<ITutorSubjectService, TutorSubjectService>();
services.AddScoped<ILessonPackagesService, LessonPackagesService>();
services.AddScoped<ITutorTasksService, TutorTasksService>();
services.AddScoped<ISolutionsService, SolutionsService>();
services.AddScoped<ITutorFeedbacksService, TutorFeedbacksService>();
services.AddScoped<IReviewsService, ReviewsService>();
services.AddScoped<IReportsService, ReportsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//Опции для куки
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.AddMappedEndpoints();

app.Run();
