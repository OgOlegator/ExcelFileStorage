using ExcelFileStorage.Api.Services.IServices;

namespace ExcelFileStorage.Api.Services
{
    public class ExcelRebuilder : IExcelRebuilder
    {
        private IFormFile _formFile;

        public async Task<IFormFile> RebuildAsync()
        {
            return _formFile;
        }

        IExcelRebuilder IExcelRebuilder.SetFile(IFormFile file)
        {
            _formFile = new FormFile(file.OpenReadStream(), 0, file.Length, file.Name, file.FileName);

            return this;
        }
    }
}
