
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var resultObject = new
        {
            ExceptionType = context.Exception.GetType().FullName,
            ExceptionMessage = context.Exception.Message,
            ExceptionStackTrace = context.Exception.StackTrace
        };
        var jsonResult = new JsonResult(resultObject)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.Result = jsonResult;
    }
}