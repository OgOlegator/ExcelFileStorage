using ExcelFileStorage.Api.Exceptions;
using ExcelFileStorage.Api.Services.IServices;
using System.IO;
using System.Net;

namespace ExcelFileStorage.Api.Services
{
    /// <summary>
    /// Обработчик файлов на сервере
    /// </summary>
    public class FileInServerService : IFileInServerService
    {
        /// <summary>
        /// Удалить файл с сервера
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="directory">Папка для поиска файла</param>
        /// <exception cref="FileNotFoundException">Файл не найден</exception>
        /// <exception cref="ExcelFileStorageException">Ошибка</exception>
        public void Delete(string fileName, string directory)
        {
            try
            {
                var filePath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), directory, fileName });

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Не найден файл {fileName}");

                File.Delete(filePath);
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ExcelFileStorageException($"Ошибка при удалении файла {fileName}", ex);
            }
        }

        /// <summary>
        /// Получить файл с сервера
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="directory">Папка для поиска файла</param>
        /// <returns>Файл</returns>
        /// <exception cref="FileNotFoundException">Файл не найден</exception>
        /// <exception cref="ExcelFileStorageException">Ошибка</exception>
        public async Task<StreamContent> GetAsync(string fileName, string directory)
        {
            try
            {
                var filePath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), directory, fileName });

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Не найден файл {fileName}");
            
                var memory = new MemoryStream();

                using (var stream = new FileStream(filePath, FileMode.Open))
                    await stream.CopyToAsync(memory);

                memory.Position = 0;

                var content = new StreamContent(memory);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(GetContentType(filePath));

                return content;
            }
            catch(FileNotFoundException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new ExcelFileStorageException($"Ошибка при получении файла {fileName}", ex);
            }
        }

        /// <summary>
        /// Сохранить файл на сервере
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="directory">Папка для сохранения</param>
        /// <exception cref="ExcelFileStorageException">Ошибка</exception>
        public async Task SaveAsync(IFormFile file, string directory)
        {
            try
            {
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), directory);

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                string fullPath = Path.Combine(pathToSave, file.FileName);

                using FileStream stream = new(fullPath, FileMode.Create);

                await file.CopyToAsync(stream);
            }
            catch (Exception ex)
            {
                throw new ExcelFileStorageException($"Ошибка при сохранении файла {file.FileName}", ex);
            }
        }

        /// <summary>
        /// Получить тип контента файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Тип контента</returns>
        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            switch (ext)
            {
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".xls":
                    return "application/vnd.ms-excel";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
