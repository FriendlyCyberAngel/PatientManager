namespace PatientManagerWebAPI.Error;


public class ErrorBody
{
  public ErrorBody(int? statusCode, string? error,
      string? message, DateTime? timeStamp)
  {
    StatusCode = statusCode;
    Error = error;
    Message = message;
    TimeStamp = timeStamp;
  }

  public ErrorBody() {}


  public int? StatusCode {get; set;}
  public string? Error {get; set;}
  public string? Message {get; set;}
  public DateTime? TimeStamp {get; set;}
}