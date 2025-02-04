namespace PatientManagerWebAPI.Service;

using PatientManagerWebAPI.Model;
using PatientManagerWebAPI.Data;
using PatientManagerWebApi.Error;

public class RecommendationService : IRecommendationService
{
  public async Task<Recommendation?> GetById(Guid id)
  {
    var recommendation = await _repository.GetById(id);

    if(recommendation != null)
      return recommendation;

    throw new DataNotFoundException("Couldn't find a recommendation with the requested id!");
  }

  public async Task<IEnumerable<Recommendation>>
      GetByPatientId(Guid patientId, int skip, int amount)
  {
    if(skip < 0)
      throw new InvalidAttributeException("Parameter 'Skip' can't be negative!");
    
    return await _repository.GetByPatientId(patientId, skip, amount);
  }

  
  public async Task<IEnumerable<Recommendation>>
      GetByPatientIdAndCompleted(Guid patientId, bool completed, int skip, int amount)
  {
    if(skip < 0)
      throw new InvalidAttributeException("Parameter 'Skip' can't be negative!");

    return await _repository.GetByPatientIdAndCompleted(
        patientId, completed, skip, amount);
  }

  public async Task<Recommendation?> Add(Recommendation recommendation)
  {
    FixAttributes(recommendation);
    ValidateNew(recommendation);
    recommendation.CreatedAt = recommendation.CreatedAt?.ToUniversalTime();
    return await _repository.Insert(recommendation);
  }

  public async Task<Recommendation?> Update(Recommendation recommendation)
  {
    FixAttributes(recommendation);
    ValidateUpdate(recommendation);
    var result = await _repository.Update(recommendation);

    if(result > 0)
      return await GetById(recommendation.Id ?? Guid.Empty);

    throw new DataNotFoundException("Couldn't find the requested recommendation to update!");
  }

  public async Task<Recommendation?> Remove(Guid id)
  {
    var recommendation = await _repository.GetById(id);

    if(recommendation != null)
    {
      var result = await _repository.Delete(id);

      if(result > 0)
        return recommendation;
    }

    throw new DataNotFoundException("Couldn't find the requested recommendation to remove!");
  }

  private void FixAttributes(Recommendation recommendation)
  {
    recommendation.Description = recommendation.Description?.Trim();
  }

  private void ValidateNew(Recommendation recommendation)
  {
    if(recommendation.PatientId == null || recommendation.PatientId == Guid.Empty)
    {
      throw new InvalidAttributeException(
          "The recommendation field 'patientId' can't be empty!");
    }

    if(recommendation.Description == null ||
        recommendation.Description.Trim().Equals(""))
    {
      throw new InvalidAttributeException(
          "The recommendation field 'description' can't be empty!");
    }
  }

  private void ValidateUpdate(Recommendation recommendation)
  {
    if("".Equals(recommendation.Description?.Trim()))
    {
      throw new InvalidAttributeException(
          "The recommendation field 'description' can't be empty!");
    }
  }

  public RecommendationService(IRecommendationRepository repository)
  {
    _repository = repository;
  }


  private readonly IRecommendationRepository _repository;
}