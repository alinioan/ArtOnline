using ArtOnline.Database.Repository;
using ArtOnline.Infrastructure.Extensions;
using ArtOnline.Services.Extensions;

namespace ArtOnline.Api;

public static class Program
{
    private const string ApplicationName = "ArtOnline";
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.AddCorsConfiguration()
            .AddRepository()
            .AddAuthorizationWithSwagger(ApplicationName)
            .AddServices()
            .UseLogger()
            .AddWorkers()
            .AddApi();

        var app = builder
            .Build()
            .ConfigureApplication(ApplicationName)
            .MigrateDatabase<WebAppDatabaseContext>();
        
        app.Run();
    }
}