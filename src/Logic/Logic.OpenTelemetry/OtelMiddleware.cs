namespace codingfreaks.OtelDemo.Logic.OpenTelemetry
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Http;

    public class OtelMiddleware : IMiddleware
    {
        #region explicit interfaces

        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("X-Activity-TraceId", Activity.Current?.TraceId.ToString() ?? "-");
                context.Response.Headers.Add("X-Activity-SpanId", Activity.Current?.SpanId.ToString() ?? "-");
                context.Response.Headers.Add("X-Activity-Id", Activity.Current?.Id ?? "-");
                return Task.CompletedTask;
            });
            await next(context);
        }

        #endregion
    }
}