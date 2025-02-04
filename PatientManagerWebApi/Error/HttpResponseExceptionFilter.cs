namespace PatientManagerWebAPI.Error;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PatientManagerWebApi.Error;


public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{

  public void OnActionExecuted(ActionExecutedContext context)
  {
    HandleDataNotFoundException(context);
    HandleInvalidAttributeException(context);
    HandleDbUpdateException(context);
    HandleDefaultException(context);
  }

  public void HandleDataNotFoundException(ActionExecutedContext context)
  {
    if(context.Exception is DataNotFoundException)
    {
      var statusCode = StatusCodes.Status404NotFound;
      var body = new ErrorBody(statusCode, "Not Found",
          context.Exception.Message, DateTime.Now);
      context.Result = new ObjectResult(body) {StatusCode = statusCode};
      context.ExceptionHandled = true;
    }
  }

  public void HandleInvalidAttributeException(ActionExecutedContext context)
  {
    if(context.Exception is InvalidAttributeException)
    {
      var statusCode = StatusCodes.Status400BadRequest;
      var body = new ErrorBody(statusCode, "Bad Request",
          context.Exception.Message, DateTime.Now);
      context.Result = new ObjectResult(body) {StatusCode = statusCode};
      context.ExceptionHandled = true;
    }
  }

  public void HandleDbUpdateException(ActionExecutedContext context)
  {
    if(context.Exception is DbUpdateException)
    {
      var statusCode = StatusCodes.Status400BadRequest;
      var body = new ErrorBody(statusCode, "Bad Request",
          context.Exception.Message, DateTime.Now);
      context.Result = new ObjectResult(body) {StatusCode = statusCode};
      context.ExceptionHandled = true;
    }
  }

  public void HandleDefaultException(ActionExecutedContext context)
  {
    if(context.Exception != null && !context.ExceptionHandled)
    {
      var statusCode = StatusCodes.Status500InternalServerError;
      var body = new ErrorBody(statusCode, "Internal Server Error",
          context.Exception?.Message ?? "Couldn't get any error message!",
          DateTime.Now);
      context.Result = new ObjectResult(body) {StatusCode = statusCode};
      context.ExceptionHandled = true;
      Console.WriteLine($"An error occurred: {context.Exception}");
    }
  }

  public void OnActionExecuting(ActionExecutingContext context) { }


  public int Order => int.MaxValue - 10;
}