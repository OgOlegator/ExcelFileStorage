using ClosedXML.Excel;
using ExcelFileStorage.Api.Exceptions;
using ExcelFileStorage.Api.Services.IServices;

namespace ExcelFileStorage.Api.Services
{
    /// <summary>
    /// Переработка Excel-файлов
    /// </summary>
    public class ExcelRebuilder : IFileRebuilder
    {
        private IFormFile _file;

        public async Task<IFormFile> BuildAsync()
        {
            try
            {
                using var workbook = new XLWorkbook(_file.OpenReadStream());

                foreach (var worksheet in workbook.Worksheets)
                    worksheet.RangeUsed().Transpose(XLTransposeOptions.MoveCells);

                var memory = new MemoryStream();

                //Не уничтожаем поток, чтобы не затереть файл до окончания запроса
                workbook.SaveAs(memory);

                return new FormFile(memory, 0, memory.Length, _file.Name, GetNewFileName(_file.FileName));
            }
            catch
            {
                throw new ExcelFileStorageException($"Ошибка при преобразовании файла {_file.FileName}");
            }
        }

        public IFileBuilder SetFile(IFormFile file)
        {
            _file = file;

            return this;
        }

        /// <summary>
        /// Получить новое имя файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Новое имя файла</returns>
        private string GetNewFileName(string fileName)
            => Path.GetFileNameWithoutExtension(fileName) + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + Path.GetExtension(fileName);
    }
}
