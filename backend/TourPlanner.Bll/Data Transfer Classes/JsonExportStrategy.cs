using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Bll.Strategies
{
    public class JsonExportStrategy : ITourExportStrategy
    {
        private readonly ILogger<JsonExportStrategy> _logger;
        public string FileExtension => "json";

        public JsonExportStrategy(ILogger<JsonExportStrategy> logger)
        {
            _logger = logger;
        }

        public byte[] Export(IEnumerable<Tour> tours)
        {
            var tourList = tours.ToList();
            _logger.LogDebug("Serialzing {Count} tours to JSON", tourList.Count);

            try
            {
                var json = JsonSerializer.Serialize(tourList, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                var bytes = Encoding.UTF8.GetBytes(json);
                _logger.LogTrace("Serialized JSON payload size: {Size} bytes", bytes.Length);
                return bytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to serialize tours to JSON");
                throw new ExportException("JSON export failed", ex);
            }
        }
    }
}
