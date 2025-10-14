using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Minio;
using VideoService.Application;
using VideoService.Application.Interfaces.Services;
using VideoService.Application.Services;
using VideoService.Infrastructura.Services;
using VideoService.Infrastructure.Data;
using VideoService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5004); // HTTP
    options.ListenAnyIP(5005, listenOptions => listenOptions.UseHttps()); // HTTPS
});


var endpoint = "minio:9000"; // Minio server endpoint
var accessKey = "minioadmin";
var secretKey = "minioadmin";
// Add Minio using the default endpoint
//builder.Services.AddMinio(accessKey, secretKey);

// Add Minio using the custom endpoint and configure additional settings for default MinioClient initialization
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(endpoint)
    .WithCredentials(accessKey, secretKey)
    .WithSSL(false) 
    .Build());
    


builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddWebApiServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration);


builder.Services.AddScoped<IMinioService, MinioService>();
builder.Services.AddScoped<IAvatarService, AvatarService>();
builder.Services.AddScoped<IVideoService, VideoWebService>();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<CustomExceptionMiddleware>();
app.UseRouting();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();