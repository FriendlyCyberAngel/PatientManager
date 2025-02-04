namespace PatientManagerWebAPI.Service;

using System.Text.RegularExpressions;
using PatientManagerWebAPI.Model;
using PatientManagerWebAPI.Data;
using PatientManagerWebApi.Error;

public class UserService : IUserService
{
  public async Task<IEnumerable<User>> GetAll(int skip, int amount)
  {
    if(skip < 0)
      throw new InvalidAttributeException("Parameter 'Skip' can't be negative!");

    return await _repository.GetAll(skip, amount);
  }

  public async Task<User?> GetById(Guid id)
  {
    var user = await _repository.GetById(id);

    if(user != null)
      return user;

    throw new DataNotFoundException("Couldn't find a user with the requested id!");
  }

  public async Task<User?> Add(User user)
  {
    FixAttributes(user);
    ValidateNew(user);
    user.CreatedAt = user.CreatedAt?.ToUniversalTime();
    return await _repository.Insert(user);
  }

  public async Task<User?> Update(User user)
  {
    FixAttributes(user);
    ValidateUpdate(user);
    var result = await _repository.Update(user);

    if(result > 0)
      return await GetById(user.Id ?? Guid.Empty);

    throw new DataNotFoundException("Couldn't find the requested user to update!");
  }

  public async Task<User?> Remove(Guid id)
  {
    var user = await _repository.GetById(id);

    if(user != null)
    {
      var result = await _repository.Delete(id);

      if(result > 0)
        return user;
    }

    throw new DataNotFoundException("Couldn't find the requested user to remove!");
  }

  private void FixAttributes(User user)
  {
    user.Name = user.Name?.Trim();
    user.PhoneNumber = user.PhoneNumber?.Trim();
    user.Email = user.Email?.ToLower().Trim();
    user.Login = user.Login?.ToLower().Trim();
  }

  private void ValidateNew(User user)
  {
    if(user.Name == null || user.Name.Trim().Equals(""))
      throw new InvalidAttributeException("The user field 'name' can't be empty!");

    if(user.PhoneNumber == null || user.PhoneNumber.Trim().Equals(""))
      throw new InvalidAttributeException("The user field 'phoneNumber' can't be empty!");

    if(user.Email == null || user.Email.Trim().Equals(""))
      throw new InvalidAttributeException("The user field 'email' can't be empty!");

    if(user.Role == null)
      throw new InvalidAttributeException("The user field 'role' can't be empty!");

    if(user.Login == null || user.Login.Trim().Equals(""))
      throw new InvalidAttributeException("The user field 'login' can't be empty!");

    if(user.Password == null || user.Password.Trim().Equals(""))
      throw new InvalidAttributeException("The user field 'password' can't be empty!");

    var regex = @"^[0-9]{3}-[0-9]{3}-[0-9]{4}$";
    var match = Regex.Match(user.PhoneNumber, regex, RegexOptions.IgnoreCase);

    if(!match.Success)
    {
      throw new InvalidAttributeException(
          "The user field 'phoneNumber' has to match the format '123-456-7890'!");
    }

    regex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    match = Regex.Match(user.Email, regex, RegexOptions.IgnoreCase);

    if(!match.Success)
    {
      throw new InvalidAttributeException(
          "The user field 'email' has to match the format 'useremail@service.com'!");
    }

    if(user.Password.Length < 8 || user.Password.Length > 20)
      throw new InvalidAttributeException(
          "The user field 'password' needs to have 8 to 20 characters'!");
  }

  private void ValidateUpdate(User user)
  {
    if("".Equals(user.Name?.Trim()))
      throw new InvalidAttributeException("The user field 'name' can't be empty!");

    if("".Equals(user.PhoneNumber?.Trim()))
      throw new InvalidAttributeException("The user field 'phoneNumber' can't be empty!");

    if("".Equals(user.Email?.Trim()))
      throw new InvalidAttributeException("The user field 'email' can't be empty!");

    if(user.PhoneNumber != null)
    {
      var regex = @"^[0-9]{3}-[0-9]{3}-[0-9]{4}$";
      var match = Regex.Match(user.PhoneNumber, regex, RegexOptions.IgnoreCase);
    
      if(!match.Success)
      {
        throw new InvalidAttributeException(
            "The user field 'phoneNumber' has to match the format '123-456-7890'!");
      }
    }

    if(user.Email != null)
    {
      var regex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
      var match = Regex.Match(user.Email, regex, RegexOptions.IgnoreCase);

      if(!match.Success)
      {
        throw new InvalidAttributeException(
            "The user field 'email' has to match the format 'useremail@service.com'!");
      }
    }
  }

  public UserService(IUserRepository repository)
  {
    _repository = repository;
  }


  private readonly IUserRepository _repository;
}