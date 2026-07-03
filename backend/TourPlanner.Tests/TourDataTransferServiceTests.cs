using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TourPlanner.Bll.Exceptions;
using TourPlanner.Bll.Interfaces;
using TourPlanner.Bll.Services;
using TourPlanner.Models;

namespace TourPlanner.Tests;

public class TourDataTransferServiceTests
{
    private ITourService _tourService;
    private ILogger<TourDataTransferService> _logger;
    private TourDataTransferService _tourDataTransferService;

    [SetUp]
    public void Setup()
    {
        _tourService = Substitute.For<ITourService>();
        _logger = Substitute.For<ILogger<TourDataTransferService>>();
    }

    #region Helper Methods
    private TourDataTransferService CreateServiceWithStrategies(
        IEnumerable<ITourExportStrategy> strategies
    )
    {
        return new TourDataTransferService(_logger, _tourService, strategies);
    }
    #endregion

    #region ExportToursAsync Tests
    [Test]
    public void ExportToursAsync_UnsupportedFormat_ThrowsUnsupportedFormatException()
    {
        // Arrange
        var service = CreateServiceWithStrategies(new List<ITourExportStrategy>());

        // Act & Assert
        Assert.ThrowsAsync<UnsupportedFormatException>(() =>
            service.ExportToursAsync("alice", "xml")
        );
    }

    public async Task ExportToursAsync_ValidFormat_CallsCorrectStrategy()
    {
        // Arrange
        var strategy = Substitute.For<ITourExportStrategy>();
        strategy.FileExtension.Returns("json");
        strategy.Export(Arg.Any<List<Tour>>()).Returns(new byte[] { 1, 2, 3 });

        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        _tourService
            .GetAllAsync("alice")
            .Returns(
                new List<Tour>
                {
                    new Tour
                    {
                        Id = 1,
                        UserId = 1,
                        Name = "Tour1",
                        Description = "Tour Desc",
                        From = location,
                        To = location,
                        Distance = 10.0,
                        EstimatedTime = 300.0,
                        TransportType = Models.Enums.TransportType.BIKE,
                        RouteInformation = String.Empty,
                    },
                }
            );

        var service = CreateServiceWithStrategies(new List<ITourExportStrategy> { strategy });

        // Act
        var result = await service.ExportToursAsync("alice", "json");

        // Assert
        Assert.That(result, Is.EqualTo(new byte[] { 1, 2, 3 }));
        strategy.Received(1).Export(Arg.Any<List<Tour>>());
    }

    [Test]
    public async Task ExportToursAsync_NoTours_StillExportsEmptyList()
    {
        // Arrange
        var strategy = Substitute.For<ITourExportStrategy>();
        strategy.FileExtension.Returns("json");
        strategy.Export(Arg.Any<List<Tour>>()).Returns(Array.Empty<byte>());

        _tourService.GetAllAsync("alice").Returns(new List<Tour>());

        var service = CreateServiceWithStrategies(new List<ITourExportStrategy> { strategy });

        // Act
        var result = await service.ExportToursAsync("alice", "json");

        // Assert
        Assert.That(result, Is.EqualTo(Array.Empty<byte>()));
        strategy.Received(1).Export(Arg.Is<List<Tour>>(t => t.Count == 0));
    }

    [Test]
    public async Task ExportToursAsync_MultipleStrategies_PicksCorrectOneByExtension()
    {
        // Arrange
        var jsonStrategy = Substitute.For<ITourExportStrategy>();
        jsonStrategy.FileExtension.Returns("json");
        jsonStrategy.Export(Arg.Any<List<Tour>>()).Returns(new byte[] { 1, 2, 3 });

        var csvStrategy = Substitute.For<ITourExportStrategy>();
        csvStrategy.FileExtension.Returns("csv");
        csvStrategy.Export(Arg.Any<List<Tour>>()).Returns(new byte[] { 4, 5, 6 });

        _tourService.GetAllAsync("alice").Returns(new List<Tour>());

        var service = CreateServiceWithStrategies(
            new List<ITourExportStrategy> { jsonStrategy, csvStrategy }
        );

        // Act
        var result = await service.ExportToursAsync("alice", "csv");

        // Assert
        Assert.That(result, Is.EqualTo(new byte[] { 4, 5, 6 }));
        csvStrategy.Received(1).Export(Arg.Any<List<Tour>>());
        jsonStrategy.DidNotReceive().Export(Arg.Any<List<Tour>>());
    }
    #endregion

    #region ImportToursAsync Tests
    [Test]
    public void ImportToursAsync_MalformedJson_ThrowsImportException()
    {
        // Arrange
        var service = CreateServiceWithStrategies(new List<ITourExportStrategy>());
        var badJson = Encoding.UTF8.GetBytes("{ malformed json");

        // Act & Assert
        Assert.ThrowsAsync<ImportException>(() => service.ImportToursAsync("alice", badJson));
    }

    [Test]
    public async Task ImportToursAsync_EmptyArray_ReturnsZeroCounts()
    {
        // Arrange
        var service = CreateServiceWithStrategies(new List<ITourExportStrategy>());
        var json = JsonSerializer.SerializeToUtf8Bytes(new List<Tour>());

        // Act
        var result = await service.ImportToursAsync("alice", json);

        // Assert
        Assert.That(result.SuccessCount, Is.EqualTo(0));
        Assert.That(result.FailCount, Is.EqualTo(0));
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public async Task ImportToursAsync_AllToursValid_AllImportsSucceed()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        var tours = new List<Tour>
        {
            new()
            {
                Id = 1,
                UserId = 1,
                Name = "Tour1",
                Description = "Tour Desc",
                From = location,
                To = location,
                Distance = 10.0,
                EstimatedTime = 300.0,
                TransportType = Models.Enums.TransportType.BIKE,
                RouteInformation = String.Empty,
            },
            new()
            {
                Id = 2,
                UserId = 1,
                Name = "Tour2",
                Description = "Tour Desc",
                From = location,
                To = location,
                Distance = 20.0,
                EstimatedTime = 600.0,
                TransportType = Models.Enums.TransportType.CAR,
                RouteInformation = String.Empty,
            },
        };
        var json = JsonSerializer.SerializeToUtf8Bytes(tours);

        _tourService.CreateTourAsync("alice", Arg.Any<Tour>()).Returns(Task.CompletedTask);

        var service = CreateServiceWithStrategies(new List<ITourExportStrategy>());

        // Act
        var result = await service.ImportToursAsync("alice", json);

        // Assert
        Assert.That(result.SuccessCount, Is.EqualTo(2));
        Assert.That(result.FailCount, Is.EqualTo(0));
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public async Task ImportToursAsync_MixOfValidAndInvalid_TracksCountsAndErrors()
    {
        Location location = Substitute.For<Location>(10, 20, "Tomatotown");

        var tours = new List<Tour>
        {
            new()
            {
                Id = 1,
                UserId = 1,
                Name = "Tour1",
                Description = "Tour Desc",
                From = location,
                To = location,
                Distance = 10.0,
                EstimatedTime = 300.0,
                TransportType = Models.Enums.TransportType.BIKE,
                RouteInformation = String.Empty,
            },
            new()
            {
                Id = 2,
                UserId = 1,
                Name = "Tour2",
                Description = "Tour Desc",
                From = location,
                To = location,
                Distance = 20.0,
                EstimatedTime = 600.0,
                TransportType = Models.Enums.TransportType.CAR,
                RouteInformation = String.Empty,
            },
        };
        var json = JsonSerializer.SerializeToUtf8Bytes(tours);

        _tourService
            .CreateTourAsync("alice", Arg.Is<Tour>(t => t.Id == 1))
            .Returns(Task.CompletedTask);
        _tourService
            .CreateTourAsync("alice", Arg.Is<Tour>(t => t.Id == 2))
            .ThrowsAsync(new TourValidationException("Simulated failure"));

        var service = CreateServiceWithStrategies(new List<ITourExportStrategy>());

        // Act
        var result = await service.ImportToursAsync("alice", json);

        // Assert
        Assert.That(result.SuccessCount, Is.EqualTo(1));
        Assert.That(result.FailCount, Is.EqualTo(1));
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].TourName, Is.EqualTo("Tour2"));
        Assert.That(result.Errors[0].Reason, Does.Contain("Simulated failure"));
    }
    #endregion
}
