using ExcelFileStorage.Api.Exceptions;
using ExcelFileStorage.Api.Filters;
using ExcelFileStorage.Api.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Buffers.Text;
using System.IO;
using System.Net;
using System.Net.Mime;

namespace ExcelFileStorage.Api.Controllers
{
    /// <summary>
    /// Хранилище Excel-файлов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelFileStorageController : ControllerBase
    {
        private readonly IFileInServerService _fileInServerHandler;
        private readonly IExcelRebuilderService _excelRebuilderService;

        public ExcelFileStorageController(IFileInServerService fileInServerHandler, IExcelRebuilderService excelRebuilderService)
        {
            _fileInServerHandler = fileInServerHandler;
            _excelRebuilderService = excelRebuilderService;
        }

        /// <summary>
        /// Загрузить файл на сервер
        /// </summary>
        /// <param name="file">Excel-файл. Поддерживаемые форматы - .xlsx/.xls </param>
        /// <returns>Имя файла на сервере</returns>
        [HttpPost]
        [Route("upload")]
        [FileValidationFilter(new string[] { ".xlsx", ".xls" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            try
            {
                var newFile = await _excelRebuilderService.SetFile(file).Rebuild();

                await _fileInServerHandler.SaveAsync(newFile, Constants.UploadsExcelFilesDirecoryName);

                return Ok(newFile.FileName);
            }
            catch (ExcelFileStorageException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Скачать файл с сервера
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <returns>Файл</returns>
        [HttpGet]
        [Route("download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadAsync(string name)
        {
            try
            {
                var content = await _fileInServerHandler.GetAsync(name, Constants.UploadsExcelFilesDirecoryName);

                return File(await content.ReadAsByteArrayAsync(), content.Headers.ContentType.MediaType, name);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ExcelFileStorageException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить файла с сервера
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <returns>Результат операции</returns>
        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            try 
            { 
                _fileInServerHandler.Delete(name, Constants.UploadsExcelFilesDirecoryName);

                return Ok();
            }
            catch (ExcelFileStorageException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
