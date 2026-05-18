using Teachly.Core.Enums;
using Teachly.Core.Models;

namespace Teachly.UnitTests
{
    internal static class TestData
    {
        public static User User(Guid? id = null, UserRole role = UserRole.Student)
        {
            return Teachly.Core.Models.User.Create(
                id ?? Guid.NewGuid(),
                "User123",
                "Ivan",
                "Ivanov",
                16,
                "IvanIvanov@mail.ru",
                "passwordHash",
                role,
                new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc)).Value;
        }

        public static Student Student(Guid? id = null, Guid? userId = null)
        {
            return Teachly.Core.Models.Student.Create(
                id ?? Guid.NewGuid(),
                userId ?? Guid.NewGuid(),
                null,
                9,
                "+79990000000").Value;
        }

        public static Tutor Tutor(Guid? id = null, Guid? userId = null)
        {
            return Teachly.Core.Models.Tutor.Create(
                id ?? Guid.NewGuid(),
                userId ?? Guid.NewGuid(),
                "Описание репетитора").Value;
        }

        public static Subject Subject(Guid? id = null)
        {
            return Teachly.Core.Models.Subject.Create(
                id ?? Guid.NewGuid(),
                "Математика").Value;
        }

        public static TutorSubject TutorSubject(Guid? id = null, Guid? tutorId = null, Guid? subjectId = null)
        {
            return Teachly.Core.Models.TutorSubject.Create(
                id ?? Guid.NewGuid(),
                tutorId ?? Guid.NewGuid(),
                subjectId ?? Guid.NewGuid(),
                500m).Value;
        }

        public static LessonPackage LessonPackage(Guid? id = null, Guid? studentId = null, Guid? tutorSubjectId = null)
        {
            return Teachly.Core.Models.LessonPackage.Create(
                id ?? Guid.NewGuid(),
                studentId ?? Guid.NewGuid(),
                tutorSubjectId ?? Guid.NewGuid(),
                4,
                500m,
                purchasedAt: new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc)).Value;
        }

        public static TutorTask TutorTask(Guid? id = null, Guid? lessonPackageId = null, DateTime? createdAt = null)
        {
            return Teachly.Core.Models.TutorTask.Create(
                id ?? Guid.NewGuid(),
                lessonPackageId ?? Guid.NewGuid(),
                "Задание",
                "Описание задания",
                createdAt ?? new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc)).Value;
        }

        public static Solution Solution(Guid? id = null, Guid? tutorTaskId = null, Guid? studentId = null, DateTime? submittedAt = null)
        {
            return Teachly.Core.Models.Solution.Create(
                id ?? Guid.NewGuid(),
                tutorTaskId ?? Guid.NewGuid(),
                studentId ?? Guid.NewGuid(),
                "Ответ",
                submittedAt ?? new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc)).Value;
        }

        public static TutorFeedback TutorFeedback(Guid? id = null, Guid? solutionId = null, short grade = 5)
        {
            return Teachly.Core.Models.TutorFeedback.Create(
                id ?? Guid.NewGuid(),
                solutionId ?? Guid.NewGuid(),
                "Комментарий",
                grade,
                new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc)).Value;
        }
    }
}
