using ExcelFileStorage.Api.Filters;
using ExcelFileStorage.Api.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;

namespace ExcelFileStorage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelFileStorageController : ControllerBase
    {
        private readonly IFileInServerHandler _fileInServerHandler;

        public ExcelFileStorageController(IFileInServerHandler fileInServerHandler)
        {
            _fileInServerHandler = fileInServerHandler;
        }

        [HttpPost]
        [CheckExcelFileFilter]
        public async Task<IResult> UploadAsync(IFormFile file)
        {
            await _fileInServerHandler.SaveAsync(file, Constants.UploadsExcelFilesDirecoryName);

            return Results.Ok();
        }

        [HttpGet]
        public async Task<IResult> GetByNameAsync(string name)
        {
            var file = _fileInServerHandler.Get(name, Constants.UploadsExcelFilesDirecoryName);

            return Results.Ok(file);
        }

        [HttpDelete]
        public async Task<IResult> DeleteAsync(string name)
        {
            _fileInServerHandler.Delete(name, Constants.UploadsExcelFilesDirecoryName);

            return Results.Ok();
        }
    }
}
