namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Сервис переработки Excel-файла
    /// </summary>
    public interface IExcelRebuilder
    {
        /// <summary>
        /// Установить файл для переработки
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns>Инстанция данного класса</returns>
        IExcelRebuilder SetFile(IFormFile file);

        /// <summary>
        /// Переработать файл
        /// </summary>
        /// <returns>Новый файл</returns>
        Task<IFormFile> RebuildAsync();
    }
}
