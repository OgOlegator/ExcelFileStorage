namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Сервис формирования файла
    /// </summary>
    public interface IFileBuilder
    {
        /// <summary>
        /// Установить файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns>Ссылку на себя</returns>
        IFileBuilder SetFile(IFormFile file);

        /// <summary>
        /// Сформировать файл
        /// </summary>
        /// <returns>Новый файл</returns>
        Task<IFormFile> BuildAsync();
    }
}
