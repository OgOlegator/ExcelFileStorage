namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Сервис переработки Excel-файла
    /// </summary>
    public interface IExcelRebuilderService
    {
        /// <summary>
        /// Установить файл для переработки
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns>Инстанция данного класса</returns>
        IExcelRebuilderService SetFile(IFormFile file);

        /// <summary>
        /// Переработать файл
        /// </summary>
        /// <returns>Новый файл</returns>
        Task<IFormFile> Rebuild();
    }
}
