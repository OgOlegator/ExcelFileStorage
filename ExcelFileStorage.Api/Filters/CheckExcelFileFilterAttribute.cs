using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mime;

namespace ExcelFileStorage.Api.Filters
{
    public class CheckExcelFileFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {            
            var errorMessages = new List<string>();

            foreach (var file in context.HttpContext.Request.Form.Files)
            {
               
            }

            if (errorMessages.Count() == 0)
                await next();

            await context.HttpContext.Response.WriteAsJsonAsync(Results.BadRequest(errorMessages));

            return;
        }
    }
}
