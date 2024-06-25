using BlazorCRUDApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Serilog;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(".", "log", "api-log-.log"), rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}")
            .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<BlazorCRUDDbContext>(dbOptions =>
    {
        dbOptions.UseSqlServer(connectString, builder => builder.MigrationsAssembly("BlazorCRUDApp.Api"));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    using (IServiceScope scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BlazorCRUDDbContext>();
        dbContext.Database.Migrate();
    }

    app.UseCors(policy =>
    {
        policy.WithOrigins("https://localhost:7012", "http://localhost:5019")
              .AllowAnyMethod()
              .WithHeaders(HeaderNames.ContentType);
    });

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "API host terminated unexceptedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}