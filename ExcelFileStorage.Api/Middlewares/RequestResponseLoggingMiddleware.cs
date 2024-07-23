using DocumentFormat.OpenXml.InkML;
using ExcelFileStorage.Api.Services.IServices;
using Microsoft.AspNetCore.Http;

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

                response = await NextAndLogResponseAsync(context);
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
                        { "DateTime", DateTime.Now },
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
            var log = new Dictionary<string, object>
            {
                { "ContentType", context.Request.ContentType },
                { "ContentLength", context.Request.ContentLength },
                { "QueryString", context.Request.QueryString},
            };

            //Если тело запроса содержит JSON -> логгируем
            if (IsBodyWithText(context.Request.ContentType))
                log.Add("Body", await GetRequestBodyAsync(context)); 

            if(context.Request.HasFormContentType && context.Request.Form.Files.Any())
                log.Add("UploadFiles", context.Request.Form.Files.Select(file => file.FileName));

            return log;
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
                { "ExceptionType", exception.GetType().Name },
                { "ExceptionMsg" , exception.Message }
            };
        }

        /// <summary>
        /// Запуск следующего этапа middleware и логгирование ответа запроса
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <returns></returns>
        private async Task<Dictionary<string, object>> NextAndLogResponseAsync(HttpContext context)
        {
            var responseBodyText = await NextAndGetResponseBodyAsync(context);

            var log = new Dictionary<string, object>
            {
                { "StatusCode", context.Response.StatusCode },
                { "ContentType", context.Response.ContentType },
                { "ContentLength", context.Response.ContentLength },
            };

            //Если тело ответа содержит JSON -> логгируем
            if(IsBodyWithText(context.Response.ContentType))
                log.Add("ResponseBody", responseBodyText);

            return log;
        }

        /// <summary>
        /// Получить тело запроса
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <returns>Тело запроса</returns>
        private async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            //Указываем, что context.Request.Body можно прочитать несколько раз. Для использования на следующих этапах обработки запроса
            context.Request.EnableBuffering();

            //Использование оператора using приведет к закрытию основного потока тела запроса/ответа по завершении блока using, и код на более позднем этапе
            //обработки запроса не сможет прочитать Body
            var requestBody = await new StreamReader(context.Request.Body, leaveOpen: true).ReadToEndAsync();

            //Возвращаем в начальную позицию для чтения Body на следующих этапах обработки запроса
            context.Request.Body.Position = 0;

            return requestBody;
        }

        /// <summary>
        /// Запуск следующего этапа middleware и поулчение тела ответа
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <returns>Тело ответа</returns>
        private async Task<string> NextAndGetResponseBodyAsync(HttpContext context)
        {
            var originalResponseBody = context.Response.Body;
            var newResponseBody = new MemoryStream();

            context.Response.Body = newResponseBody;

            await _next(context);

            newResponseBody.Seek(0, SeekOrigin.Begin);

            //Использование оператора using приведет к закрытию основного потока тела запроса/ответа по завершении блока using, и код на более позднем этапе
            //обработки запроса не сможет прочитать Body
            var responseBodyText = await new StreamReader(newResponseBody).ReadToEndAsync();

            newResponseBody.Seek(0, SeekOrigin.Begin);

            //Чтобы избежать затирание потока нужно скопировать начальное состояние
            await newResponseBody.CopyToAsync(originalResponseBody);

            return responseBodyText;
        }

        private bool IsBodyWithText(string contentType)
            => contentType == "application/json; charset=utf-8"
            || contentType == "text/plain; charset=utf-8";
    }
}
