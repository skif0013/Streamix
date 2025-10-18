﻿    using System.ComponentModel.DataAnnotations;

namespace VideoService.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}, TraceId: {context.TraceIdentifier}, Path: {context.Request.Path}");

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string errorMessage;
            object? errorDetails = null;

            if (exception is ValidationException validationException)
            {
                statusCode = StatusCodes.Status200OK;
                errorMessage = "Validation failed";

               
            }
            else if (exception is Minio.Exceptions.MinioException minioEx)
            {
                statusCode = StatusCodes.Status400BadRequest;
                errorMessage = "Storage error: " + minioEx.Message;
            }
            else
            {
                statusCode = StatusCodes.Status400BadRequest;
                errorMessage = exception.Message;
            }

            var result = Result<object>.Failure(errorMessage);
            result.Data = new
            {
                traceId = context.TraceIdentifier,
                path = context.Request.Path,
                errors = errorDetails 
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(result);
        }

    }
}
