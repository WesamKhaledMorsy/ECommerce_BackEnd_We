using Microsoft.AspNetCore.Http.Features;
using Store.Service.HandleResponses;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Store.Web.Middlewares
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// RequestDelegate >>> necessary to inject it in the constructor
        public ExceptionMiddleware(RequestDelegate next,
            IHostEnvironment environment,
            ILogger<ExceptionMiddleware> logger
            )
        {
            _next=next;
            _environment=environment;
            _logger=logger;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                // try the coming request
                await _next(context); // If there is no issuses will continue
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType= "application/json";
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError; //500

                var response = _environment.IsDevelopment()
                    ? new CustomException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new CustomException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                // Serialize => convert the object to json 
                var json = JsonSerializer.Serialize(response, options);
                // write in the response the json that we Serialized
                await context.Response.WriteAsync(json);
            }
        }
    }
}
