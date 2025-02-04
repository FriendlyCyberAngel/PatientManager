namespace PatientManagerWebAPI.Data;

using PatientManagerWebAPI.Model;


public interface IRecommendationRepository
{
  Task<Recommendation?> GetById(Guid id);
  Task<IEnumerable<Recommendation>> GetByPatientId(Guid patientId, int skip, int amount);
  Task<IEnumerable<Recommendation>>
      GetByPatientIdAndCompleted(Guid patientId, bool completed, int skip, int amount);
  Task<Recommendation?> Insert(Recommendation recommendation);
  Task<int> Update(Recommendation recommendation);
  Task<int> Delete(Guid id);
}