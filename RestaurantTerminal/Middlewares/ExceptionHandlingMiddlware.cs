using Newtonsoft.Json;
using System.Net;

namespace RestaurantTerminal.Middlewares
{
    public class ExceptionHandlingMiddlware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddlware> _logger;

        public ExceptionHandlingMiddlware(RequestDelegate next, ILogger<ExceptionHandlingMiddlware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(KeyNotFoundException ex)
            {
                await HandleExceptionAsync(
                    context,
                    ex.Message,
                    HttpStatusCode.NotFound,
                    "Страница не найдена");
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(
                    context,
                    ex.Message,
                    HttpStatusCode.InternalServerError,
                    "Ошибка сервера, пожалуйста попробуйте позже");
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            string exMsg,
            HttpStatusCode statusCode,
            string msg)
        {
            _logger.LogError(exMsg);

            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;

            string result = JsonConvert.SerializeObject(new {
                Message = msg,
                StatusCode = (int)statusCode
            });

            await response.WriteAsync(result.ToString());
        }

    }
}
