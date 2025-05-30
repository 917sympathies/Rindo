namespace Rindo.API.Middleware.Authentication;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // TODO: Get rid of AsyncActionAccessFilter and transport it's implementation here
        await _next(context);
    }
}