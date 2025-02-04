namespace PatientManagerWebAPI.Request;


public record UpdatePatientRequest(
  string? Name,
  DateOnly? Birthdate,
  string? PhoneNumber,
  string? Email
);
