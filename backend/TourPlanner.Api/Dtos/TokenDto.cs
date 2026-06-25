using System.ComponentModel.DataAnnotations;

namespace TourPlanner.Api.Dtos
{
    public class TokenDto
    {
        [Required]
        public required string Token { get; set; }
    }
}
