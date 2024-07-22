using ClosedXML.Excel;
using ExcelFileStorage.Api.Services.IServices;

namespace ExcelFileStorage.Api.Services
{
    public class ExcelRebuilder : IExcelRebuilder
    {
        /// <summary>
        /// Переработка входящего Excel-файла - транспонирование
        /// </summary>
        /// <param name="file">Excel-файл</param>
        /// <returns>Новый файл</returns>
        public async Task<IFormFile> RebuildAsync(IFormFile file)
        {
            using var workbook = new XLWorkbook(file.OpenReadStream());

            foreach(var worksheet in workbook.Worksheets)
                worksheet.RangeUsed().Transpose(XLTransposeOptions.MoveCells);

            var memory = new MemoryStream();

            workbook.SaveAs(memory);

            return new FormFile(memory, 0, memory.Length, file.Name, file.FileName);
        }
    }
}
