using ExcelFileStorage.Api.Services.IServices;

namespace ExcelFileStorage.Api.Middlewares
{
    /// <summary>
    /// Логгирование запросов и ответов к API в конвейере обработки запроса
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAppLogger logger)
        {
            Dictionary<string, object> request = null;
            Dictionary<string, object> response = null;
            Dictionary<string, object> error = null;

            try
            {
                request = await LogRequestAsync(context);

                await _next(context);

                response = await LogResponseAsync(context);
            }
            catch (Exception ex)
            {
                error = LogError(ex);

                throw;
            }
            finally
            {
                logger.Log(
                    $"Method: {context.Request.Method}. Path: {context.Request.Path}",
                    new Dictionary<string, object>
                    {
                        { "Request", request},
                        { "Response", response },
                        { "Error", error }
                    });
            }
        }

        /// <summary>
        /// Логгирование запроса 
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <returns></returns>
        private async Task<Dictionary<string, object>> LogRequestAsync(HttpContext context)
        {
            //Указываем, что context.Request.Body можно прочитать несколько раз. Для использования на следующих этапах обработки запроса
            context.Request.EnableBuffering();

            //Использование оператора using приведет к закрытию основного потока тела запроса/ответа по завершении блока using, и код на более позднем этапе
            //обработки запроса не сможет прочитать Body
            var requestBody = await new StreamReader(context.Request.Body, leaveOpen: true).ReadToEndAsync();

            //Возвращаем в начальную позицию для чтения Body на следующих этапах обработки запроса
            context.Request.Body.Position = 0;

            return new Dictionary<string, object>
            {
                { "DateTime", DateTime.Now },
                { "QueryString", context.Request.QueryString},
                { "RequestBody", requestBody },
            };
        }

        /// <summary>
        /// Логгирование ошибок
        /// </summary>
        /// <param name="exception">Объект ошибки</param>
        /// <returns></returns>
        private Dictionary<string, object> LogError(Exception exception)
        {
            return new Dictionary<string, object>
            {
                { "DateTime", DateTime.Now },
                { "ExceptionType", exception.GetType().Name },
                { "ExceptionMsg" , exception.Message }
            };
        }

        /// <summary>
        /// Запуск следующего шага обработки запроса и логгирование ответа запроса
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <returns></returns>
        private async Task<Dictionary<string, object>> LogResponseAsync(HttpContext context)
        {
            return new Dictionary<string, object>
            {
                { "DateTime", DateTime.Now },
                { "StatusCode", context.Response.StatusCode },
            };
        }
    }
}
