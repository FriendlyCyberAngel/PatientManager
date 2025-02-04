namespace PatientManagerWebAPI.Request;


public record UpdateRecommendationRequest(
  string? Description,
  bool? Completed
);