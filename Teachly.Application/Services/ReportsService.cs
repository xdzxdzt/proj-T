using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;

namespace Teachly.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly ISolutionsRepository _solutionsRepository;
        private readonly ITutorFeedbacksRepository _tutorFeedbacksRepository;

        public ReportsService(
            IStudentsRepository studentsRepository,
            ISolutionsRepository solutionsRepository,
            ITutorFeedbacksRepository tutorFeedbacksRepository)
        {
            _studentsRepository = studentsRepository;
            _solutionsRepository = solutionsRepository;
            _tutorFeedbacksRepository = tutorFeedbacksRepository;
        }

        //Доделать
        public async Task<(Guid StudentId, int SubmittedSolutions, int CheckedSolutions, double AverageGrade)> GetStudentProgressReport(Guid studentId)
        {
            var student = await _studentsRepository.GetById(studentId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var solutions = await _solutionsRepository.GetAllByStudentId(student.Id);

            return (student.Id, 11, 11, 11);
        }
    }
}
