using Teachly.API.Contracts.Dictionaries;
using Teachly.Application.Interfaces.Repositories;

namespace Teachly.API.Endpoints
{
    public static class DictionariesEndpoints
    {
        public static IEndpointRouteBuilder MapDictionariesEndpoints(this IEndpointRouteBuilder app)
        {
            var dictionaries = app.MapGroup("dictionaries");

            dictionaries.MapGet("subjects", GetSubjects);
            dictionaries.MapGet("institutions", GetInstitutions);

            return app;
        }

        private static async Task<IResult> GetSubjects(ISubjectsRepository subjectsRepository)
        {
            var subjects = await subjectsRepository.GetAll();
            var response = subjects
                .OrderBy(s => s.Name)
                .Select(s => new SubjectResponse(s.Id, s.Name))
                .ToList();

            return Results.Ok(response);
        }

        private static async Task<IResult> GetInstitutions(IInstitutionsRepository institutionsRepository)
        {
            var institutions = await institutionsRepository.GetAll();
            var response = institutions
                .OrderBy(i => i.City)
                .ThenBy(i => i.Name)
                .Select(i => new InstitutionResponse(
                    i.Id,
                    i.Type.ToString(),
                    i.Name,
                    i.City))
                .ToList();

            return Results.Ok(response);
        }
    }
}
