namespace PatientManagerWebAPI.Data;

using PatientManagerWebAPI.Model;


public interface IUserRepository
{
  Task<IEnumerable<User>> GetAll(int skip, int amount);
  Task<User?> GetById(Guid id);
  Task<User?> Insert(User user);
  Task<int> Update(User user);
  Task<int> Delete(Guid id);
}