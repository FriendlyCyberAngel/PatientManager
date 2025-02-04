namespace PatientManagerWebAPI.Data;

using Microsoft.EntityFrameworkCore;
using PatientManagerWebAPI.Model;


public class PatientRepository : IPatientRepository
{
  public async Task<IEnumerable<Patient>> GetAll(int skip, int amount)
  {
    return await _appDbContext.Patient.OrderBy(p => p.Name).
        Skip(skip).Take(amount).AsNoTracking().ToListAsync();
  }

  public async Task<IEnumerable<Patient>> GetByName(string name, int skip, int amount)
  {
    return await _appDbContext.Patient.
        Where(p => p.Name.ToLower().Contains(name.ToLower())).
        OrderBy(p => p.Name).Skip(skip).Take(amount).
        AsNoTracking().ToListAsync();
  }

  public async Task<Patient?> GetById(Guid id)
  {
    return await _appDbContext.Patient.FindAsync(id);
  }

  public async Task<Patient?> Insert(Patient patient)
  {
    var result = await _appDbContext.Patient.AddAsync(patient);
    var rows = await _appDbContext.SaveChangesAsync();
    return rows > 0 ? result.Entity : null;
  }

  public async Task<int> Update(Patient patient)
  {
    return await _appDbContext.Patient.Where(fp => fp.Id == patient.Id).
        ExecuteUpdateAsync(up =>
            up.SetProperty(p => p.Name, p => patient.Name ?? p.Name)
              .SetProperty(p => p.Birthdate, p => patient.Birthdate ?? p.Birthdate)
              .SetProperty(p => p.PhoneNumber, p => patient.PhoneNumber ?? p.PhoneNumber)
              .SetProperty(p => p.Email, p => patient.Email ?? p.Email));
  }

  public async Task<int> Delete(Guid id)
  {
    return await _appDbContext.Patient.Where(u => u.Id == id).ExecuteDeleteAsync();
  }

  public PatientRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }


  private readonly AppDbContext _appDbContext;
}