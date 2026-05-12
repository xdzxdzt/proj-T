using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.Profiles
{
    public record TutorProfileResponse(
        [Required]
        Guid TutorId,

        [Required]
        Guid UserId,

        [Required]
        [MaxLength(User.MAX_USERNAME_LENGTH)]
        string UserName,

        [Required]
        [MaxLength(User.MAX_FIRSTNAME_LENGTH)]
        string FirstName,

        [Required]
        [MaxLength(User.MAX_LASTNAME_LENGTH)]
        string LastName,

        [Required]
        [Range(1, int.MaxValue)]
        int Age,

        [Required]
        [EmailAddress]
        [MaxLength(User.MAX_EMAIL_LENGTH)]
        string Email,

        [MaxLength(User.MAX_AVATAR_URL_LENGTH)]
        string? AvatarUrl,

        [MaxLength(Tutor.MAX_DESCRIPTION_LENGTH)]
        string? Description,

        [Required]
        [Range(0, 5)]
        decimal AverageRating,

        [Required]
        [Range(0, int.MaxValue)]
        int RatingCount);
}
