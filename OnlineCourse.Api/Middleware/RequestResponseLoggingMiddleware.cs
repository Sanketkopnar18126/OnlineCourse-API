using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace OnlineCourse.Api.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 1️⃣ Log request
            context.Request.EnableBuffering();

            string requestBody;
            using (var reader = new StreamReader(
                context.Request.Body,
                encoding: System.Text.Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset for next middleware
            }

            Log.Information(
                "Request {method} {url} => Body: {body}",
                context.Request.Method,
                context.Request.Path,
                requestBody
            );

            // 2️⃣ Capture response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context); // Call next middleware/controller

            // 3️⃣ Log response
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            Log.Information(
                "Response {statusCode} => Body: {body}",
                context.Response.StatusCode,
                responseText
            );

            // 4️⃣ Copy the response back to the original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
