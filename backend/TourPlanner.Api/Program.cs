using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using TourPlanner.Dal;

namespace TourPlanner.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(
                new WebApplicationOptions { Args = args, WebRootPath = "public" }
            );

            builder.WebHost.UseWebRoot("public");

            // Health checks

            builder.Services.AddHealthChecks();

            // Add db context + repos

            builder
                .Services.AddOptions<Configuration.DatabaseOptions>()
                .Bind(builder.Configuration.GetSection("ConnectionStrings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddDbContext<TourPlannerDbContext>(
                (serviceProvider, options) =>
                {
                    Configuration.DatabaseOptions dbOptions = serviceProvider
                        .GetRequiredService<IOptions<Configuration.DatabaseOptions>>()
                        .Value;

                    options.UseNpgsql(dbOptions.DBConn);
                }
            );

            builder.Services.AddScoped<
                Dal.Interfaces.ITourRepository,
                Dal.DatabaseRepositories.TourRepository
            >();
            builder.Services.AddScoped<
                Dal.Interfaces.ITourLogRepository,
                Dal.DatabaseRepositories.TourLogRepository
            >();

            // Add services to the container.
            builder.Services.AddScoped<Bll.Interfaces.ITourService, Bll.Services.TourService>();
            builder.Services.AddScoped<
                Bll.Interfaces.ITourLogService,
                Bll.Services.TourLogService
            >();
            builder.Services.AddScoped<
                Bll.Interfaces.IContactService,
                Bll.Services.ContactService
            >();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseDefaultFiles();
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), "public")
                    ),
                }
            );

            app.MapFallbackToFile("index.html");

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
