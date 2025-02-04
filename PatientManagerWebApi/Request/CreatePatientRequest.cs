namespace PatientManagerWebAPI.Request;


public record CreatePatientRequest(
  string Name,
  DateOnly Birthdate,
  string PhoneNumber,
  string Email
);
