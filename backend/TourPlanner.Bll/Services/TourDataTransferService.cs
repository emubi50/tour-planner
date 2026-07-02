using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Bll.Strategies;
using TourPlanner.Models;

namespace TourPlanner.Bll.Services
{
    public class TourDataTransferService : ITourDataTransferService
    {
        private readonly ILogger<TourDataTransferService> _logger;
        private readonly ITourService _tourService;
        private readonly Dictionary<string, ITourExportStrategy> _exportStrategies;

        public TourDataTransferService(
            ILogger<TourDataTransferService> logger,
            ITourService tourService,
            IEnumerable<ITourExportStrategy> exportStrategies
        )
        {
            _logger = logger;
            _tourService = tourService;
            _exportStrategies = exportStrategies.ToDictionary(s => s.FileExtension.ToLower(), s => s);

            _logger.LogInformation("TourDataTransferService initialized with {Count} strategies: {Strategies}",
                _exportStrategies.Count, string.Join(", ", _exportStrategies.Keys)
            );
        }

        public async Task<byte[]> ExportToursAsync(string username, string format)
        {
            _logger.LogInformation("Export requested for user {Username}: format={Format}", username, format);
        
            if (!_exportStrategies.TryGetValue(format.ToLower(), out var strategy))
            {
                _logger.LogWarning("Unsupported export format requested: {Format}", format);
                throw new UnsupportedFormatException(format);
            }

            List<Tour> tours = await _tourService.GetAllAsync(username);
            if (!tours.Any())
            {
                _logger.LogWarning("User {Username} has no tours to export.", username);
            }

            _logger.LogDebug("Delegating export of {Count} tours to strategy {Strategy}", tours.Count, strategy.GetType().Name);

            var result = strategy.Export(tours);
            _logger.LogInformation(
                "Export completed for username {Username}, {Bytes} bytes produced",
                username,
                result.Length);
            return result;
        }

        public async Task<ImportResult> ImportToursAsync(string username, byte[] fileContent)
        {
            _logger.LogInformation(
                "Import started for username {Username}: payload size={Size} bytes",
                username,
                fileContent.Length);

            var result = new ImportResult();

            List<Tour> parsedTours;
            try
            {
                var json = Encoding.UTF8.GetString(fileContent);
                parsedTours = JsonSerializer.Deserialize<List<Tour>>(json) ?? new List<Tour>();
                _logger.LogDebug("Parsed {Count} tours from import file", parsedTours.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse import file for username {Username}", username);
                throw new ImportException("Could not parse import file", ex);
            }

            foreach (var tour in parsedTours)
            {
                try
                {
                    _logger.LogTrace("Attempting to import tour: {Name}", tour.Name);
                    await _tourService.CreateTourAsync(username, tour);
                    result.SuccessCount++;
                }
                catch (TourValidationException ex)
                {
                    _logger.LogWarning(
                        "Skipping invalid tour '{Name}' during import: {Reason}",
                        tour.Name,
                        ex.Message);
                    result.FailCount++;
                    result.Errors.Add(new ImportError { TourName = tour.Name, Reason = ex.Message });
                }
            }

            _logger.LogInformation(
                "Import finished for user {Username}: {Success} succeeded, {Failed} failed",
                username,
                result.SuccessCount,
                result.FailCount);

            return result;
        }
    }
}
