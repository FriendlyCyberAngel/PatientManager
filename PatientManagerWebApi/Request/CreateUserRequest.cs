namespace PatientManagerWebAPI.Request;

using PatientManagerWebAPI.Model;


public record CreateUserRequest(
  string Name,
  string PhoneNumber,
  string Email,
  Role Role,
  string Login
);