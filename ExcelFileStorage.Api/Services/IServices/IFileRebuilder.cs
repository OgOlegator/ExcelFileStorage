namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Сервис переработки файла
    /// </summary>
    public interface IFileRebuilder
    {
        /// <summary>
        /// Переработать файл
        /// </summary>
        /// <returns>Новый файл</returns>
        Task<IFormFile> RebuildAsync(IFormFile file);
    }
}
