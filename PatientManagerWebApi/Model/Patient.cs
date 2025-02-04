namespace PatientManagerWebAPI.Model;


public class Patient
{
  public Patient(Guid? id, string? name, DateOnly? birthdate,
      string? phoneNumber, string? email, DateTime? createdAt)
  {
    Id = id;
    Name = name;
    Birthdate = birthdate;
    PhoneNumber = phoneNumber;
    Email = email;
    CreatedAt = createdAt;
  }

  public Patient() {}


  public Guid? Id {get; set;}

  public string? Name {get; set;}

  public DateOnly? Birthdate {get; set;}

  public string? PhoneNumber {get; set;}

  public string? Email {get; set;}
  public DateTime? CreatedAt {get; set;}
}