using System.Net;
namespace AssurAmiBackEnd.Core.Entity.Authentification
{
    public class CustomForbiddenMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomForbiddenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden || context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\": \"vous n'aver pas l'autorisation pour accéder a ce URL.\"}");
            }
        }
    }
}
