using System.Diagnostics;

namespace CSharpApp.Api.Middlewares
{
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMiddleware> _logger;

        public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var requestPath = context.Request.Path;

            _logger.LogInformation("Request [{RequestPath}] executed in {ElapsedMilliseconds}ms", requestPath, elapsedMs);
        }
    }
}
