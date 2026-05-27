using Microsoft.EntityFrameworkCore;
using Timer.Infrastructure.DependencyInjection;
using timer.Database;
using Timer.Infrastructure.DependencyInjection.RateLimiting;
using timer.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// builder.Services.AddOpenApi();

builder.Services
    .AddSwagger()
    .AddHttpContextAccessor()
    .AddTimerServices(builder.Configuration)
    .AddAuth(builder.Configuration)
    .AddDatabase(builder.Configuration)
    .AddRateLimiting(builder.Configuration)
    .TimerAddCors();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";

builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

app.TimerUseCors();

if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger(app.Environment);
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();