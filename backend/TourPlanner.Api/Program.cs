using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TourPlanner.Models.Options;
using TourPlanner.Dal;
using TourPlanner.Models;

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
            builder.Services.AddOptions<JwtSettings>()
                .Bind(builder.Configuration.GetSection("JwtSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // OpenRouteService settings
            builder.Services.AddOptions<OpenRouteServiceOptions>()
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

            builder.Services.AddControllers();

            builder
                .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSection = builder.Configuration.GetSection("JWT");
                    var signingKey = jwtSection["SigningKey"]!;
                    var issuer = jwtSection["Issuer"]!;
                    var audience = jwtSection["Audience"]!;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(signingKey)
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
