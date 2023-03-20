using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using System.Security.Claims;

namespace Graphene.Server
{
    public class HttpRequestInterceptor : DefaultHttpRequestInterceptor
    {
        public override ValueTask OnCreateAsync(HttpContext context,
            IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {
            string? userId =
                context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            requestBuilder.SetGlobalState("UserId", userId);

            return base.OnCreateAsync(context, requestExecutor, requestBuilder,
                cancellationToken);
        }
    }
}
