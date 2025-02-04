
namespace PatientManagerWebAPI.Data;

using Microsoft.EntityFrameworkCore;
using PatientManagerWebAPI.Model;


public class RecommendationRepository : IRecommendationRepository
{
  public async Task<IEnumerable<Recommendation>>
      GetByPatientId(Guid patientId, int skip, int amount)
  {
    return await _appDbContext.Recommendation.
        Where(r => r.PatientId == patientId).OrderByDescending(u => u.CreatedAt).
        Skip(skip).Take(amount).AsNoTracking().ToListAsync();
  }

  public async Task<IEnumerable<Recommendation>>
      GetByPatientIdAndCompleted(Guid patientId, bool completed, int skip, int amount)
  {
    return await _appDbContext.Recommendation.
        Where(r => r.PatientId == patientId && r.Completed == completed).
        OrderByDescending(u => u.CreatedAt).Skip(skip).Take(amount).
        AsNoTracking().ToListAsync();
  }

  public async Task<Recommendation?> GetById(Guid id)
  {
    return await _appDbContext.Recommendation.FindAsync(id);
  }

  public async Task<Recommendation?> Insert(Recommendation recommendation)
  {
    var result = await _appDbContext.Recommendation.AddAsync(recommendation);
    var rows = await _appDbContext.SaveChangesAsync();
    return rows > 0 ? result.Entity : null;
  }

  public async Task<int> Update(Recommendation recommendation)
  {
    return await _appDbContext.Recommendation.Where(fr => fr.Id == recommendation.Id).
        ExecuteUpdateAsync(ur =>
            ur.SetProperty(r => r.Description, r => recommendation.Description ?? r.Description)
              .SetProperty(r => r.Completed, r => recommendation.Completed ?? r.Completed ?? false));
  }

  public async Task<int> Delete(Guid id)
  {
    return await _appDbContext.Recommendation.Where(r => r.Id == id).ExecuteDeleteAsync();
  }

  public RecommendationRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }


  private readonly AppDbContext _appDbContext;
}