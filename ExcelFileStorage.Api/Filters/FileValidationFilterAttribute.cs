using ExcelFileStorage.Api.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExcelFileStorage.Api.Filters
{
    /// <summary>
    /// Фильтр действий для проверки файлов
    /// </summary>
    public class FileValidationFilterAttribute : Attribute, IActionFilter
    {
        private string[] _allowedExtensions;

        public FileValidationFilterAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Реализация не требуется
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var errorMessages = new List<string>();

            foreach (var file in context.HttpContext.Request.Form.Files)
                if (!FileValidator.IsFileExtensionAllowed(file, _allowedExtensions))
                    errorMessages.Add($"Файл {file.FileName} имеет неподдерживаемый тип для вызываемого действия");

            if (errorMessages.Count() == 0)
                return;

            context.Result = new BadRequestObjectResult(errorMessages);
        }
    }
}
