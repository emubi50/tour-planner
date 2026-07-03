using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TourPlanner.Api.Middleware;
using TourPlanner.Bll.Services;
using TourPlanner.Bll.Strategies;
using TourPlanner.Dal;
using TourPlanner.Models.Options;

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

            // Global Exception Handler
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            // Log4Net Magic
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddLog4Net("log4net.config");

            // Health checks

            builder.Services.AddHealthChecks();

            // Add db context + repos

            builder
                .Services.AddOptions<DatabaseOptions>()
                .Bind(builder.Configuration.GetSection("ConnectionStrings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddDbContext<TourPlannerDbContext>(
                (serviceProvider, options) =>
                {
                    DatabaseOptions dbOptions = serviceProvider
                        .GetRequiredService<IOptions<DatabaseOptions>>()
                        .Value;

                    options.UseNpgsql(dbOptions.DBConn);
                }
            );

            // JWT settings
            builder
                .Services.AddOptions<JwtSettings>()
                .Bind(builder.Configuration.GetSection("Jwt"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // OpenRouteService settings
            builder
                .Services.AddOptions<OpenRouteServiceOptions>()
                .Bind(builder.Configuration.GetSection("OpenRouteService"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddScoped<
                Dal.Interfaces.ITourRepository,
                Dal.DatabaseRepositories.TourRepository
            >();
            builder.Services.AddScoped<
                Dal.Interfaces.ITourLogRepository,
                Dal.DatabaseRepositories.TourLogRepository
            >();

            builder.Services.AddScoped<
                Dal.Interfaces.IUserRepository,
                Dal.DatabaseRepositories.UserRepository
            >();

            builder.Services.AddHttpClient<
                Bll.Interfaces.IOpenRouteService,
                Bll.Services.OpenRouteService
            >(client =>
            {
                client.BaseAddress = new Uri("https://api.openrouteservice.org/");
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        builder.Configuration["OpenRouteService:ApiKey"]
                    );
            });

            // Add services to the container.
            builder.Services.AddScoped<Bll.Interfaces.ITourService, Bll.Services.TourService>();
            builder.Services.AddScoped<
                Bll.Interfaces.ITourLogService,
                Bll.Services.TourLogService
            >();
            builder.Services.AddScoped<Bll.Interfaces.IUserService, Bll.Services.UserService>();
            builder.Services.AddScoped<
                Bll.Interfaces.IPasswordHashingService,
                Bll.Services.PasswordHashingService
            >();
            builder.Services.AddScoped<Bll.Interfaces.ITokenService, Bll.Services.TokenService>();
            builder.Services.AddScoped<Bll.Interfaces.ITourExportStrategy, JsonExportStrategy>();
            builder.Services.AddScoped<
                Bll.Interfaces.ITourDataTransferService,
                TourDataTransferService
            >();

            builder.Services.AddControllers();

            builder
                .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings =
                        builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
                        ?? throw new InvalidOperationException("JWT settings are not configured.");
                    ;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SigningKey)
                        ),
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["token"];
                            return Task.CompletedTask;
                        },
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TourPlannerDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseExceptionHandler();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
