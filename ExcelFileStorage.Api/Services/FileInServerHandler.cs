using ExcelFileStorage.Api.Services.IServices;

namespace ExcelFileStorage.Api.Services
{
    /// <summary>
    /// Обработчик файлов на сервере
    /// </summary>
    public class FileInServerHandler : IFileInServerHandler
    {
        public void Delete(string fileName, string directory)
        {
            throw new NotImplementedException();
        }

        public IFormFile Get(string fileName, string directory)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(IFormFile file, string directory)
        {
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), directory);

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            string fullPath = Path.Combine(pathToSave, file.FileName);

            using FileStream stream = new(fullPath, FileMode.Create);

            await file.CopyToAsync(stream);
        }
    }
}
