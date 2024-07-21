namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Формирование отчетов от сайта https://httpbin.org/
    /// </summary>
    public interface IHttpbinReportBuilder
    {
        /// <summary>
        /// Сформировать отчет
        /// </summary>
        /// <param name="file">Файл для отправки</param>
        Task BuildAsync(IFormFile file);
    }
}
