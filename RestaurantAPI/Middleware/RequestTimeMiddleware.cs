
using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private ILogger<RequestTimeMiddleware> _logger;
        private Stopwatch _stopWatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopWatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            var elapsedMilliseconds = _stopWatch.ElapsedMilliseconds;
            if(elapsedMilliseconds / 1000 > 4)
            {
                var path = context.Request.Path;
                var query = context.Request.QueryString;
                var method = context.Request.Method;
                var message = $"Request [{method}] at {path}{query} took {elapsedMilliseconds} ms";
                
                _logger.LogInformation(message);
            }
        }
    }
}
