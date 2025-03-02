namespace CSharpApp.Infrastructure.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics;
    using System.Threading.Tasks;

    namespace CSharpApp.Infrastructure.Middlewares
    {
        public class PerformanceLoggingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<PerformanceLoggingMiddleware> _logger;

            public PerformanceLoggingMiddleware(RequestDelegate next, ILogger<PerformanceLoggingMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    await _next(context);
                }
                finally
                {
                    stopwatch.Stop();
                    _logger.LogInformation(
                        "Request: {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds
                    );
                }
            }
        }
    }

}
