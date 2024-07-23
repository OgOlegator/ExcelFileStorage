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
        /// <summary>
        /// Переработка входящего Excel-файла - транспонирование
        /// </summary>
        /// <param name="file">Excel-файл</param>
        /// <returns>Новый файл</returns>
        /// <exception cref="ExcelFileStorageException">Ошибка</exception>
        public async Task<IFormFile> RebuildAsync(IFormFile file)
        {
            try
            {
                using var workbook = new XLWorkbook(file.OpenReadStream());

                foreach (var worksheet in workbook.Worksheets)
                    worksheet.RangeUsed().Transpose(XLTransposeOptions.MoveCells);

                var memory = new MemoryStream();

                //Не уничтожаем поток, чтобы не затереть файл до окончания запроса
                workbook.SaveAs(memory);

                return new FormFile(memory, 0, memory.Length, file.Name, GetNewFileName(file.FileName));
            }
            catch 
            {
                throw new ExcelFileStorageException($"Ошибка при преобразовании файла {file.FileName}");
            }
        }

        private string GetNewFileName(string fileName)
            => Path.GetFileNameWithoutExtension(fileName) + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + Path.GetExtension(fileName);
    }
}
