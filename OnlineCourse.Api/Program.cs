using System.Net;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Api.Middleware;
using OnlineCourse.Data;
using OnlineCourse.Data.Entities;
using OnlineCourse.Service;
using Serilog;
using Serilog.Templates;

namespace OnlineCourse.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initial Serilog bootstrap logger (for startup logs)
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Debug()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                var configuration = builder.Configuration;

                // DB Context
                builder.Services.AddDbContextPool<OnlineCourseDbContext>(options =>
                {
                    options.UseSqlServer(
                        configuration.GetConnectionString("DbContext"),
                        providerOptions => providerOptions.EnableRetryOnFailure());
                });

                // Application Insights
                builder.Services.AddApplicationInsightsTelemetry();

                // Full Serilog config
                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .WriteTo.Console(new ExpressionTemplate(
                        "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} " +
                        "({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
                    .WriteTo.ApplicationInsights(
                        services.GetRequiredService<TelemetryConfiguration>(),
                        TelemetryConverter.Traces));

                // Controllers & Swagger
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                // CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowFrontend", policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });

                // Dependency Injection
                builder.Services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
                builder.Services.AddScoped<ICourseCategoryService, CourseCategoryService>();
                builder.Services.AddScoped<ICourseRepository, CourseRepository>();
                builder.Services.AddScoped<ICourseService, CourseService>();

                var app = builder.Build();

                // Exception Handler Middleware (global)
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occurred.");
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                    });
                });

                // Swagger first in dev to avoid logging its requests
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                // Request/Response Logging Middleware
                app.UseMiddleware<RequestResponseLoggingMiddleware>();

                app.UseHttpsRedirection();
                app.UseCors("AllowFrontend");
                app.UseAuthorization();
                app.MapControllers();

                Log.Information("Starting Online Course API...");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
