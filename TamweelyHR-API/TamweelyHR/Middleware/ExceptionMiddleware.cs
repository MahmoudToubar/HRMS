using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/problem+json";


            if (ex is ValidationException validationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var problem = new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation errors"
                };

                await context.Response.WriteAsJsonAsync(problem);
                return;
            }

            if (ex is DbUpdateException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = "Duplicate value or database constraint violation"
                };

                await context.Response.WriteAsJsonAsync(problem);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var serverProblem = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Server Error",
                Detail = _env.IsDevelopment() ? ex.Message : null
            };

            await context.Response.WriteAsJsonAsync(serverProblem);
        }
    }
}
