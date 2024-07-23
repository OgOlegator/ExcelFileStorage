using ExcelFileStorage.Api.Exceptions;
using ExcelFileStorage.Api.Filters;
using ExcelFileStorage.Api.Services;
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
        private readonly IFileOnServer _fileOnServer;
        private readonly IFileRebuilder _excelRebuilder;
        private readonly IHttpbinReportBuilder _httpbinReportBuilder;

        public ExcelFileStorageController(IFileOnServer fileOnServer, IFileRebuilder excelRebuilder, IHttpbinReportBuilder httpbinReportBuilder)
        {
            _fileOnServer = fileOnServer;
            _excelRebuilder = excelRebuilder;
            _httpbinReportBuilder = httpbinReportBuilder;
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
                var newFile = await _excelRebuilder.RebuildAsync(file);

                await _fileOnServer.SaveAsync(newFile, Constants.UploadsExcelFilesDirecoryName);

                await _httpbinReportBuilder.BuildAsync(newFile);                

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
                var content = await _fileOnServer.GetAsync(name, Constants.UploadsExcelFilesDirecoryName);

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
        public IActionResult Delete(string name)
        {
            try 
            { 
                _fileOnServer.Delete(name, Constants.UploadsExcelFilesDirecoryName);

                return Ok();
            }
            catch (ExcelFileStorageException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
