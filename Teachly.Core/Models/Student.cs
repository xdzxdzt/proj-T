using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class Student
    {
        public const int MAX_LENGTH_PARENTPHONE = 100;

        private Student(Guid id, Guid userId, Guid? institutionId, int educationLevel, string? parentPhone)
        {
            Id = id;
            UserId = userId;
            InstitutionId = institutionId;
            EducationLevel = educationLevel;
            ParentPhone = parentPhone;
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid? InstitutionId { get; private set; }
        public int EducationLevel { get; private set; }
        public string? ParentPhone { get; private set; }

        public static Result<Student> Create(Guid id, Guid userId, Guid? institutionId, int educationLevel, string? parentPhone)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<Student>($"'{nameof(id)}' не может быть пустым");
            }
            if (userId == Guid.Empty)
            {
                return Result.Failure<Student>($"'{nameof(userId)}' не может быть пустым");
            }
            if (educationLevel <= 0)
            {
                return Result.Failure<Student>($"'{nameof(educationLevel)}' должен быть больше нуля");
            }
            if (parentPhone is not null && parentPhone.Length > MAX_LENGTH_PARENTPHONE)
            {
                return Result.Failure<Student>($"'{nameof(parentPhone)}' не может быть длиннее {MAX_LENGTH_PARENTPHONE} символов");
            }

            var student = new Student(id, userId, institutionId, educationLevel, parentPhone);

            return Result.Success(student);
        }
    }
}
