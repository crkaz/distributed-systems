using DistSysACW.Models;
using DistSysACW.Facades;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserContext dbContext)
        {
            const string apiKeyHeader = "ApiKey";
            string apiKey = string.Empty;
            if (context.Request.Headers.TryGetValue(apiKeyHeader, out var headerValues))
            {
                apiKey = headerValues.FirstOrDefault(); // Extract from headerValues array.

                string requestedEndpoint = context.Request.Path;
                Log log = new Log(requestedEndpoint);

                DbAccessFacade.ModifyDb(() =>
                {
                    dbContext.Users.Find(apiKey).Logs.Add(log);
                    dbContext.SaveChanges();
                });
            }

            await _next(context);
        }

    }
}
