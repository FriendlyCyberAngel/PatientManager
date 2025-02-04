namespace PatientManagerWebAPI.Request;

using PatientManagerWebAPI.Model;


public record UserResponse(
  Guid? Id,
  string? Name,
  string? PhoneNumber,
  string? Email,
  Role? Role,
  DateTime? CreatedAt
);
