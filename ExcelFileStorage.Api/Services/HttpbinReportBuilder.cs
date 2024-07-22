using ExcelFileStorage.Api.Services.IServices;
using System;
using System.Buffers.Text;
using System.IO;
using System.Text.Json;

namespace ExcelFileStorage.Api.Services
{
    public class HttpbinReportBuilder : IHttpbinReportBuilder
    {
        private readonly IFileOnServer _fileOnServer;
        private readonly IHttpClientFactory _httpClient;

        public HttpbinReportBuilder(IHttpClientFactory httpClientFactory, IFileOnServer fileOnServer)
        {
            _fileOnServer = fileOnServer;
            _httpClient = httpClientFactory;
        }

        public async Task BuildAsync(IFormFile file)
        {
            var fileInBase64 = ConvertFileToBase64(file);

            var report = await GetReportAsync(fileInBase64);

            _fileOnServer.CreateOrWriteToEnd(GetFileName(file.FileName), Constants.HttpbinResponsesDirecoryName, report);
        }

        /// <summary>
        /// Преобразовать файл в base64
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns>Файл в base64</returns>
        private string ConvertFileToBase64(IFormFile file)
        {
            using var memoryStream = new MemoryStream();

            file.CopyTo(memoryStream);

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// Поулчение отчета со стороннего сервиса
        /// </summary>
        /// <param name="data">Данные для отчета</param>
        /// <returns>Отчет</returns>
        private async Task<string> GetReportAsync(string data)
        {
            var client = _httpClient.CreateClient("HttpbinReportBuilder");

            var message = new HttpRequestMessage();

            message.Headers.Add("Accept", "application/json");

            message.RequestUri = new Uri("https://httpbin.org/post");
            message.Method = HttpMethod.Post;
            message.Content = new StringContent(data);

            var apiResponse = await client.SendAsync(message);

            var statusCode = apiResponse.StatusCode;
            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            return JsonSerializer.Serialize(new
            {
                statusCode,
                responseContent
            });
        }

        /// <summary>
        /// Формирование имени файла отчета
        /// </summary>
        /// <param name="sourceFileName">Имя файла, для которого формируется отчет</param>
        /// <returns>Имя файла отчета</returns>
        private string GetFileName(string sourceFileName)
            => Path.GetFileNameWithoutExtension(sourceFileName) + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".txt";
    }
}
