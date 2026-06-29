using System.ComponentModel.DataAnnotations;

namespace TourPlanner.Models.Options
{
    public class JwtSettings
    {
        [Required, MinLength(32)]
        public string SigningKey { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int ExpirationMinutes { get; set; } = 60;
    }
}
