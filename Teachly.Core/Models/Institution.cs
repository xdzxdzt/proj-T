using CSharpFunctionalExtensions;
using Teachly.Core.Enums;

namespace Teachly.Core.Models
{
    public class Institution
    {
        public const int MAX_LENGTH_NAME = 200;
        public const int MAX_LENGTH_CITY = 100;

        private Institution(Guid id, InstitutionType type, string name, string city)
        {
            Id = id;
            Type = type;
            Name = name;
            City = city;
        }

        public Guid Id { get; private set; }
        public InstitutionType Type { get; private set; }
        public string Name { get; private set; }
        public string City { get; private set; }

        public static Result<Institution> Create(Guid id, InstitutionType type, string name, string city)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<Institution>($"'{nameof(id)}' не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_LENGTH_NAME)
            {
                return Result.Failure<Institution>($"'{nameof(name)}' не может быть пустым или длиннее {MAX_LENGTH_NAME} символов");
            }
            if (string.IsNullOrWhiteSpace(city) || city.Length > MAX_LENGTH_CITY)
            {
                return Result.Failure<Institution>($"'{nameof(city)}' не может быть пустым или длиннее {MAX_LENGTH_CITY} символов");
            }

            var institution = new Institution(id, type, name, city);

            return Result.Success(institution);
        }
    }
}
