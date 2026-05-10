using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly IStudentsRepository _studentsRepository;

        public ReviewsService(
            IReviewsRepository reviewsRepository, 
            ITutorsRepository tutorsRepository,
            IStudentsRepository studentsRepository)
        {
            _reviewsRepository = reviewsRepository;
            _tutorsRepository = tutorsRepository;
            _studentsRepository = studentsRepository;
        }

        public async Task CreateReview(
            Guid tutorId,
            Guid studentId,
            string reviewText, 
            short rating)
        {
            var tutor = await _tutorsRepository.GetById(tutorId);

            if(tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

            var student = await _studentsRepository.GetById(studentId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var alreadyExists = await _reviewsRepository.Exists(student.Id, tutor.Id);

            if(alreadyExists)
            {
                throw new InvalidOperationException("Отзыв уже существует");
            }

            var review = Review.Create(
                Guid.NewGuid(),
                student.Id,
                tutor.Id,
                reviewText,
                rating);

            if(review.IsFailure)
            {
                throw new InvalidOperationException(review.Error);
            }

            var addReviewResult = tutor.AddRating(rating);

            if (addReviewResult.IsFailure)
            {
                throw new InvalidOperationException(addReviewResult.Error);
            }

            await _reviewsRepository.Add(review.Value);
            await _tutorsRepository.Update(tutor);
        }
        
    }
}
