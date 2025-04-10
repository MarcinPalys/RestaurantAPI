using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using RestaurantAPI;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using RestaurantAPI.Services;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    var authenticationSettings = new AuthenticationSettings();

    builder.Services.AddSingleton(authenticationSettings);
    builder.Configuration.Bind("Authentication", authenticationSettings);
    // Add services to the container.

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Bearer";
        options.DefaultChallengeScheme = "Bearer";
        options.DefaultScheme = "Bearer";
    }).AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
        };
    });

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers().AddFluentValidation();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", builder => builder.RequireClaim("Nationality", "German", "Polish"));
        options.AddPolicy("atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
    });
    builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
    builder.Services.AddDbContext<RestaurantDbContext>();
    builder.Services.AddScoped<RestaurantSeeder>();
    builder.Services.AddAutoMapper(typeof(RestaurantMappingProfile));
    builder.Services.AddScoped<IRestaurantService, RestaurantService>();
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<IDishService, DishService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
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

    app.UseAuthentication();
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
