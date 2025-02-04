namespace PatientManagerWebAPI.Model;


public class Recommendation
{
  public Recommendation(Guid? id, Guid? patientId,
      string? description, bool? completed, DateTime? createdAt)
  {
    Id = id;
    PatientId = patientId;
    Description = description;
    Completed = completed;
    CreatedAt = createdAt;
  }

  public Recommendation() {}


  public Guid? Id {get; set;}
  public Guid? PatientId {get; set;}
  public string? Description {get; set;}
  public bool? Completed {get; set;}
  public DateTime? CreatedAt {get; set;}
}
