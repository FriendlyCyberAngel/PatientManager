
namespace PatientManagerWebAPI.Data;

using Microsoft.EntityFrameworkCore;
using PatientManagerWebAPI.Model;


public class UserRepository : IUserRepository
{
  public async Task<IEnumerable<User>> GetAll(int skip, int amount)
  {
    return await _appDbContext.User.OrderBy(u => u.Name).
        Skip(skip).Take(amount).AsNoTracking().ToListAsync();
  }

  public async Task<User?> GetById(Guid id)
  {
    return await _appDbContext.User.FindAsync(id);
  }

  public async Task<User?> Insert(User user)
  {
    var result = await _appDbContext.User.AddAsync(user);
    var rows = await _appDbContext.SaveChangesAsync();
    return rows > 0 ? result.Entity : null;
  }

  public async Task<int> Update(User user)
  {
    return await _appDbContext.User.Where(fu => fu.Id == user.Id).
        ExecuteUpdateAsync(uu =>
            uu.SetProperty(u => u.Name, u => user.Name ?? u.Name)
              .SetProperty(u => u.PhoneNumber, u => user.PhoneNumber ?? u.PhoneNumber)
              .SetProperty(u => u.Email, u => user.Email ?? u.Email)
              .SetProperty(u => u.Role, u => user.Role ?? u.Role));
  }

  public async Task<int> Delete(Guid id)
  {
    return await _appDbContext.User.Where(u => u.Id == id).ExecuteDeleteAsync();
  }

  public UserRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }


  private readonly AppDbContext _appDbContext;
}