using DotNetEnv;
using EmailService.Inrerfaces;
using EmailService.Middleware;
using EmailService.Model;
using EmailService.Services;

var builder = WebApplication.CreateBuilder(args);


Env.Load();


builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>();
builder.Services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();
builder.Services.AddScoped<IEmailService, EmailService.Services.EmailService>();

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


app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();