using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class Subject
    {
        public const int MAX_NAME_LENGTH = 100;

        private Subject(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public static Result<Subject> Create(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<Subject>($"'{nameof(id)}' не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
            {
                return Result.Failure<Subject>($"'{nameof(name)}' не может быть пустым или длиннее {MAX_NAME_LENGTH} символов");
            }

            var subject = new Subject(id, name);

            return Result.Success(subject);
        }
    }
}
