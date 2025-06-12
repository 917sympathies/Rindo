namespace Rindo.API.Middleware.Authentication;

public class TokenMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // TODO: Get rid of AsyncActionAccessFilter and transport it's implementation here
        await next(context);
    }
}