namespace PatientManagerWebAPI.Service;

using PatientManagerWebAPI.Model;


public interface IUserService
{
  Task<IEnumerable<User>> GetAll(int skip, int amount);
  Task<User?> GetById(Guid id);
  Task<User?> Add(User user);
  Task<User?> Update(User user);
  Task<User?> Remove(Guid id);
}