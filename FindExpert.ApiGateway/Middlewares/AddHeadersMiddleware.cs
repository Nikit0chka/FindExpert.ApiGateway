using System.Security.Claims;
using JetBrains.Annotations;


namespace FindExpert.ApiGateway.Middlewares;

public sealed class AddHeadersMiddleware(RequestDelegate next)
{
    [UsedImplicitly]
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            await next(context);
            return;
        }

        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = context.User.FindAll(ClaimTypes.Role).Select(static role => role.Value);
        context.Request.Headers.Append("X-User-Id", userId);
        context.Request.Headers.Append("X-User-Role", string.Join(", ", userRole));

        await next(context);
    }
}