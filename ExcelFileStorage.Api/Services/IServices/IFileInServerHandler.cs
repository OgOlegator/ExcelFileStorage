namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Обработчик файлов на сервере
    /// </summary>
    public interface IFileInServerHandler
    {
        /// <summary>
        /// Сохранить файл на сервере
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="directory">Папка для сохранения</param>
        Task SaveAsync(IFormFile file, string directory);

        /// <summary>
        /// Получить файл с сервера
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="directory">Папка для поиска файла</param>
        /// <returns>Файл</returns>
        IFormFile Get(string fileName, string directory);

        /// <summary>
        /// Удалить файл с сервера
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="directory">Папка для поиска файла</param>
        void Delete(string fileName, string directory);
    }
}
