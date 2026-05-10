using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.Auth
{
    public record LoginUserRequest(
        [Required]
        [EmailAddress]
        [MaxLength(User.MAX_EMAIL_LENGTH)]
        string Email,

        [Required]
        string Password);
}
