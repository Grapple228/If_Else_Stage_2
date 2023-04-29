using Database;
using WebApi.Helpers;

namespace WebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var app = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            }).Build();

        CreateDbIfNotExist(app);
        
        app.Run();
    }

    private static void CreateDbIfNotExist(IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DatabaseContext>();
        context.ConfigureDatabase();
    }
}

