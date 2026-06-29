using System.ComponentModel.DataAnnotations;

namespace TourPlanner.Models.Options;

public class DatabaseOptions
{
    [Required]
    public string DBConn { get; set; } = string.Empty;
}