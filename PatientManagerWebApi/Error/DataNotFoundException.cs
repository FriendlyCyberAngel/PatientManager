namespace PatientManagerWebApi.Error;


public class DataNotFoundException : Exception
{
  public DataNotFoundException() {}

  public DataNotFoundException(string message) : base(message) {}

  public DataNotFoundException(string message, Exception inner): base(message, inner) {}
}