namespace ABC.Users.Middleware;

public class HttpSessionMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _next(httpContext);
    }
}