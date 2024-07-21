namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Обработчик файлов на сервере
    /// </summary>
    public interface IFileOnServer
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
        Task<StreamContent> GetAsync(string fileName, string directory);

        /// <summary>
        /// Удалить файл с сервера
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="directory">Папка для поиска файла</param>
        void Delete(string fileName, string directory);

        /// <summary>
        /// Создать файл или записать в конец файла на сервере. Для текстовых файлов
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="directory">Папка для поиска файла</param>
        /// <param name="data">Данные для записи</param>
        void CreateOrWriteToEnd(string fileName, string directory, string data);
    }
}
