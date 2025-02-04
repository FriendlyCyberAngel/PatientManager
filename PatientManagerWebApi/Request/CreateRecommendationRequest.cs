namespace PatientManagerWebAPI.Request;


public record CreateRecommendationRequest(
  Guid PatientId,
  string Description,
  bool Completed
);