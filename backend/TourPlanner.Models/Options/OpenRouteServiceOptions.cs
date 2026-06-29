using System.ComponentModel.DataAnnotations;

namespace TourPlanner.Models.Options
{
    public class OpenRouteServiceOptions
    {
        [Required]
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.openrouteservice.org";
    }
}
