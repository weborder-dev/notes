using Microsoft.Extensions.Caching.Memory;

namespace NotesBackend.Extensions;

public static class RouterExtensions
{
    public static RouteHandlerBuilder WithIdemPotentValidation<TRequest>(
        this RouteHandlerBuilder builder)
    {
        builder.Add(ep =>
        {
            var originalDelegate = ep.RequestDelegate;

            ep.RequestDelegate = async context =>
            {
                if (!context.Request.Headers.TryGetValue("X-REQ-ID", out var rid))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return;
                }

                if (!Guid.TryParse(rid, out var _))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("ReqID is not a guid or is missing.");
                    return;
                }

                var cache = context.RequestServices.GetService<IMemoryCache>();
                if (cache is null)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return;
                }

                var cachedReq = cache.Get(rid);
                if (cachedReq is not null)
                {
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    await context.Response.WriteAsync("Request already prossesed");
                    return;
                }

                await originalDelegate!(context);

                cache.Set(
                    rid,
                    rid,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
                    });

                return;
            };
        });

        return builder;
    }
}