using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.Auth
{
    public record RegisterTutorRequest(
        [Required]
        [MaxLength(User.MAX_USERNAME_LENGTH)]
        string UserName,

        [Required]
        [MaxLength(User.MAX_FIRSTNAME_LENGTH)]
        string FirstName,

        [Required]
        [MaxLength(User.MAX_LASTNAME_LENGTH)]
        string LastName,

        [Range(1, int.MaxValue)]
        int Age,

        [Required]
        [EmailAddress]
        [MaxLength(User.MAX_EMAIL_LENGTH)]
        string Email,

        [Required]
        string Password,

        [MaxLength(Tutor.MAX_DESCRIPTION_LENGTH)]
        string? Description);
}
