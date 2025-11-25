namespace UserService.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleErrorAsync(context, "Ошибка клиента", ex.Message, 400);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(context, "Ошибка сервера", ex.Message, 500);
            }
        }

        private async Task HandleErrorAsync(
            HttpContext context, string exceptionInfo, string exceptionMessage, int statusCode
            )
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            ErrorDetails errorDetails = new ErrorDetails()
            {
                ExceptionInfo = exceptionInfo,
                ExceptionMessage = exceptionMessage,
                StatusCode = statusCode
            };
            await context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}
