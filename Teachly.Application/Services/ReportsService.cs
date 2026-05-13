using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Application.Reports;

namespace Teachly.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly ILessonPackagesRepository _lessonPackagesRepository;
        private readonly ISolutionsRepository _solutionsRepository;
        private readonly ITutorFeedbacksRepository _tutorFeedbacksRepository;

        public ReportsService(
            IStudentsRepository studentsRepository,
            ITutorsRepository tutorsRepository,
            ILessonPackagesRepository lessonPackagesRepository,
            ISolutionsRepository solutionsRepository,
            ITutorFeedbacksRepository tutorFeedbacksRepository)
        {
            _studentsRepository = studentsRepository;
            _tutorsRepository = tutorsRepository;
            _lessonPackagesRepository = lessonPackagesRepository;
            _solutionsRepository = solutionsRepository;
            _tutorFeedbacksRepository = tutorFeedbacksRepository;
        }

        public async Task<StudentProgressReport> GetStudentProgressReportForStudent(Guid userId)
        {
            var student = await _studentsRepository.GetByUserId(userId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            return await BuildStudentProgressReport(student.Id);
        }

        public async Task<StudentProgressReport> GetStudentProgressReportForTutor(Guid userId, Guid studentId)
        {
            var tutor = await _tutorsRepository.GetByUserId(userId);

            if (tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

            var student = await _studentsRepository.GetById(studentId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var tutorLessonPackages = await _lessonPackagesRepository.GetByTutorId(tutor.Id);
            var hasAccessToStudent = tutorLessonPackages.Any(x => x.StudentId == student.Id);

            if (!hasAccessToStudent)
            {
                throw new InvalidOperationException("Репетитор не может сформировать отчет этого обучающегося");
            }

            return await BuildStudentProgressReport(student.Id);
        }

        private async Task<StudentProgressReport> BuildStudentProgressReport(Guid studentId)
        {
            var student = await _studentsRepository.GetById(studentId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var solutions = await _solutionsRepository.GetAllByStudentId(student.Id);
            var grades = new List<short>();

            foreach (var solution in solutions)
            {
                var feedback = await _tutorFeedbacksRepository.GetBySolutionId(solution.Id);

                if (feedback is not null)
                {
                    grades.Add(feedback.Grade);
                }
            }

            var checkedSolutions = grades.Count;
            var averageGrade = checkedSolutions == 0
                ? 0
                : Math.Round(grades.Average(x => x), 2);

            return new StudentProgressReport(
                student.Id,
                solutions.Count,
                checkedSolutions,
                averageGrade);
        }
    }
}
