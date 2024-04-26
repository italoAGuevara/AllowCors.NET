
namespace AllowCors
{
    public class IPAllowCorsMiddleware 
    { 

        private readonly RequestDelegate _next;

        public IPAllowCorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

            Console.WriteLine("ipAddress : " + remoteIpAddress);

            //PUT HERE YOU'RE CUSTOM VALIDATION

            return _next(httpContext);
        }
    }

}
