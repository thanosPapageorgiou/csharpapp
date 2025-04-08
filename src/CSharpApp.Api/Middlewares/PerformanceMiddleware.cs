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
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var watch = Stopwatch.StartNew();

                await _next(context);

                watch.Stop();

                var elapsedMs = watch.ElapsedMilliseconds;
                var requestPath = context.Request.Path;

                _logger.LogInformation("Request [{RequestPath}] executed in {ElapsedMilliseconds}ms", requestPath, elapsedMs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in PerformanceMiddleware.InvokeAsync: {ex.Message}");
            }
            
        }
    }
}
