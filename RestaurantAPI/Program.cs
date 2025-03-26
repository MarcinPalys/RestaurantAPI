using NLog;
using NLog.Web;
using RestaurantAPI;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Services;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddDbContext<RestaurantDbContext>();
    builder.Services.AddScoped<RestaurantSeeder>();
    builder.Services.AddAutoMapper(typeof(RestaurantMappingProfile));
    builder.Services.AddScoped<IRestaurantService, RestaurantService>();
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<IDishService, DishService>();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI v1"));
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();



    seeder.Seed();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Aplikacja zatrzymana z powodu b³êdu.");
    throw;
}
finally
{
    LogManager.Shutdown();
}
