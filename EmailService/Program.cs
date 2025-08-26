using DotNetEnv;
using EmailService.Inrerfaces;
using EmailService.Middleware;
using EmailService.Model;
using EmailService.Services;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5003); 
    options.ListenAnyIP(5004);
});

Env.Load();


builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("ConfirmMail.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>();
builder.Services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();
builder.Services.AddScoped<IEmailService, EmailService.Services.EmailService>();

builder.Services.Configure<EmailSettings>(options =>
{
    options.Host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? throw new ArgumentNullException("SMTP_HOST is not set");
    options.Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
    options.Username = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? throw new ArgumentNullException("SMTP_USERNAME is not set");
    options.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? throw new ArgumentNullException("SMTP_PASSWORD is not set");
    options.EnableSsl = bool.Parse(Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL") ?? "true");
    options.SenderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL") ?? throw new ArgumentNullException("SENDER_EMAIL is not set");
});

builder.Services.AddControllers();


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


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();