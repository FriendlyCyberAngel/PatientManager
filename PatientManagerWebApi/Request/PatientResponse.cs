namespace PatientManagerWebAPI.Request;


public record PatientResponse(
  Guid? Id,
  string? Name,
  DateOnly? Birthdate,
  string? PhoneNumber,
  string? Email,
  DateTime? CreatedAt
);
