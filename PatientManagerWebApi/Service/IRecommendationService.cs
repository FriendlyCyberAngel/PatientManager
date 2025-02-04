namespace PatientManagerWebAPI.Service;

using PatientManagerWebAPI.Model;


public interface IRecommendationService
{
  Task<Recommendation?> GetById(Guid id);
  Task<IEnumerable<Recommendation>> GetByPatientId(
      Guid patientId, int skip, int amount);
  Task<IEnumerable<Recommendation>> GetByPatientIdAndCompleted(
      Guid patientId, bool completed, int skip, int amount);
  Task<Recommendation?> Add(Recommendation recommendation);
  Task<Recommendation?> Update(Recommendation recommendation);
  Task<Recommendation?> Remove(Guid id);
}