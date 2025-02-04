namespace PatientManagerWebApi.Error;


public class InvalidAttributeException : Exception
{
  public InvalidAttributeException() {}

  public InvalidAttributeException(string message) : base(message) {}

  public InvalidAttributeException(string message, Exception inner): base(message, inner) {}
}