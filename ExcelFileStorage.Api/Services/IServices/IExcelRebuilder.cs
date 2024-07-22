namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Сервис переработки Excel-файла
    /// </summary>
    public interface IExcelRebuilder
    {
        /// <summary>
        /// Переработать файл
        /// </summary>
        /// <returns>Новый файл</returns>
        Task<IFormFile> RebuildAsync(IFormFile file);
    }
}
