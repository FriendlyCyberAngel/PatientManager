namespace PatientManagerWebAPI.Request;

using PatientManagerWebAPI.Model;


public record UpdateUserRequest(
  string? Name,
  string? PhoneNumber,
  string? Email,
  Role? Role
);