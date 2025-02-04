namespace PatientManagerWebAPI.Controller;

using Microsoft.AspNetCore.Mvc;


[ApiController]
public class ErrorController : ControllerBase
{
  [Route("api/error")]
  public IActionResult SendError()
  {
    return Problem();
  }
}
