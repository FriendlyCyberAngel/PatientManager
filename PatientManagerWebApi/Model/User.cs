namespace PatientManagerWebAPI.Model;


public class User
{
  public User(Guid? id, string? name, string? phoneNumber, string? email,
      Role? role, string? login, string? password, DateTime? createdAt)
  {
    Id = id;
    Name = name;
    PhoneNumber = phoneNumber;
    Email = email;
    Role = role;
    Login = login;
    Password = password;
    CreatedAt = createdAt;
  }

  public User() {}


  public Guid? Id {get; set;}
  public string? Name {get; set;}
  public string? PhoneNumber {get; set;}
  public string? Email {get; set;}
  public Role? Role {get; set;}
  public string? Login {get; set;}
  public string? Password {get; set;}
  public DateTime? CreatedAt {get; set;}
}