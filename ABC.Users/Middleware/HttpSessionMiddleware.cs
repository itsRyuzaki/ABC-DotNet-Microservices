namespace ABC.Users.Middleware;

public class HttpSessionMiddleware(RequestDelegate _next, ILogger<HttpSessionMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        _logger.LogInformation("In HttpSessionMiddleware");
        
        await _next(httpContext);

        _logger.LogInformation("Processed Response, exiting from HttpSessionMiddleware");
    }
}