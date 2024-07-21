using ExcelFileStorage.Api.Services.IServices;

namespace ExcelFileStorage.Api.Services
{
    public class ExcelRebuilderService : IExcelRebuilderService
    {
        public Task<IFormFile> Rebuild()
        {
            throw new NotImplementedException();
        }

        IExcelRebuilderService IExcelRebuilderService.SetFile(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
