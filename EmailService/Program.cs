using EmailService.Inrerfaces;
using EmailService.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IEmailService, EmailService.Services.EmailService>();
builder.Services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Streamix API",
        Version = "v1",
        Description = "None",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Daniil",
            Email = "danilmiadzve@gmail.com",
        }
    });
});


var app = builder.Build();



app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<EmailService.Middleware.ErrorHandlingMiddleware>();

 // app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();