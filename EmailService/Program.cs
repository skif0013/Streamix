using DotNetEnv;
using EmailService.Inrerfaces;
using EmailService.Middleware;
using EmailService.Model;
using EmailService.Services;

var builder = WebApplication.CreateBuilder(args);

// Загрузка .env файла
Env.Load();

// Конфигурация
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("ConfirmMail.json", optional: true)
    .AddEnvironmentVariables();
// Регистрация сервисов
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>();
builder.Services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();
builder.Services.AddScoped<IEmailService, EmailService.Services.EmailService>();

// Контроллеры
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "EmailService API",
        Version = "v1",
        Description = "API for email sending",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Daniil",
            Email = "danilmiadzve@gmail.com",
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();