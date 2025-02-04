namespace PatientManagerWebAPI.Request;


public record RecommendationResponse(
  Guid? Id,
  Guid? PatientId,
  string? Description,
  bool? Completed,
  DateTime? CreatedAt
);