using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Teachly.Core.Models;

namespace Teachly.API.Contracts.Reviews
{
    public record AddReviewRequest(
        [Required]
        Guid TutorId,
        [Required]
        Guid StudentId,
        [Required]
        [MaxLength(Review.MAX_LENGTH_REVIEWTEXT)]
        string ReviewText,
        [Required]
        [Range(1,5)]
        short Rating);
}
