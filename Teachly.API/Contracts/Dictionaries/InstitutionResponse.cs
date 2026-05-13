namespace Teachly.API.Contracts.Dictionaries
{
    public record InstitutionResponse(
        Guid Id,
        string Type,
        string Name,
        string City);
}
