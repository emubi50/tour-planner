namespace TourPlanner.Api.Dtos
{
    public class LocationDto
    {
        public double[] Coordinates { get; set; } = default!;
        public string Label { get; set; } = string.Empty;
    }
}
