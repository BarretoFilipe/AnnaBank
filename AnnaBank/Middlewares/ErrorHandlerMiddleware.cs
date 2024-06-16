using AnnaBank.Abstractions;
using AnnaBank.Exceptions;
using System.Net;
using System.Text.Json;

namespace AnnaBank.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case DomainException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                Error errorDTO = new((HttpStatusCode)response.StatusCode, error?.Message);

                var result = JsonSerializer.Serialize(errorDTO);
                await response.WriteAsync(result);
            }
        }
    }
}