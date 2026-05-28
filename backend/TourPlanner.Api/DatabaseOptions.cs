using System.ComponentModel.DataAnnotations;

namespace TourPlanner.Api.Configuration;

public class DatabaseOptions
{
    [Required]
    public required string DBConn {get; init;}
}