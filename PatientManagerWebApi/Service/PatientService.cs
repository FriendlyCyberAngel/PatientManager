namespace PatientManagerWebAPI.Service;

using System.Text.RegularExpressions;
using PatientManagerWebAPI.Model;
using PatientManagerWebAPI.Data;
using PatientManagerWebApi.Error;


public class PatientService : IPatientService
{
  public async Task<IEnumerable<Patient>> GetAll(int skip, int amount)
  {
    if(skip < 0)
      throw new InvalidAttributeException("Parameter 'Skip' can't be negative!");

    return await _repository.GetAll(skip, amount);
  }

  public async Task<IEnumerable<Patient>> GetByName(string name, int skip, int amount)
  {
    if(skip < 0)
      throw new InvalidAttributeException("Parameter 'Skip' can't be negative!");

    return await _repository.GetByName(name, skip, amount);
  }

  public async Task<Patient?> GetById(Guid id)
  {
    var patient = await _repository.GetById(id);

    if(patient != null)
      return patient;

    throw new DataNotFoundException("Couldn't find a patient with the requested id!");
  }

  public async Task<Patient?> Add(Patient patient)
  {
    FixAttributes(patient);
    ValidateNew(patient);
    patient.CreatedAt = patient.CreatedAt?.ToUniversalTime();
    return await _repository.Insert(patient);
  }

  public async Task<Patient?> Update(Patient patient)
  {
    FixAttributes(patient);
    ValidateUpdate(patient);
    var result = await _repository.Update(patient);

    if(result > 0)
      return await GetById(patient.Id ?? Guid.Empty);

    throw new DataNotFoundException("Couldn't find the requested patient to update!");
  }

  public async Task<Patient?> Remove(Guid id)
  {
    var patient = await _repository.GetById(id);

    if(patient != null)
    {
      var result = await _repository.Delete(id);

      if(result > 0)
        return patient;
    }

    throw new DataNotFoundException("Couldn't find the requested patient to remove!");
  }

  private void FixAttributes(Patient patient)
  {
    patient.Name = patient.Name?.Trim();
    patient.PhoneNumber = patient.PhoneNumber?.Trim();
    patient.Email = patient.Email?.ToLower().Trim();
  }

  private void ValidateNew(Patient patient)
  {
    if(patient.Name == null || patient.Name.Trim().Equals(""))
      throw new InvalidAttributeException("The patient field 'name' can't be empty!");

    if(patient.Birthdate == null)
      throw new InvalidAttributeException("The patient field 'birthdate' can't be empty!");

    if(patient.PhoneNumber == null || patient.PhoneNumber.Trim().Equals(""))
      throw new InvalidAttributeException("The patient field 'phoneNumber' can't be empty!");

    if(patient.Email == null || patient.Email.Trim().Equals(""))
      throw new InvalidAttributeException("The patient field 'email' can't be empty!");

    var dtbd = patient.Birthdate?.ToDateTime(TimeOnly.MinValue);

    if(DateTime.Now.CompareTo(dtbd) < 0)
    {
      throw new InvalidAttributeException(
          "The patient field 'birthdate' can't be a date in the future!");
    }

    var regex = @"^[0-9]{3}-[0-9]{3}-[0-9]{4}$";
    var match = Regex.Match(patient.PhoneNumber, regex, RegexOptions.IgnoreCase);

    if(!match.Success)
    {
      throw new InvalidAttributeException(
          "The patient field 'phoneNumber' has to match the format '123-456-7890'!");
    }

    regex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    match = Regex.Match(patient.Email, regex, RegexOptions.IgnoreCase);

    if(!match.Success)
    {
      throw new InvalidAttributeException(
          "The patient field 'email' has to match the format 'patientemail@service.com'!");
    }
  }

  private void ValidateUpdate(Patient patient)
  {
    if("".Equals(patient.Name?.Trim()))
      throw new InvalidAttributeException("The patient attribute 'name' can't be empty!");

    if("".Equals(patient.PhoneNumber?.Trim()))
      throw new InvalidAttributeException("The patient attribute 'phoneNumber' can't be empty!");

    if("".Equals(patient.Email?.Trim()))
      throw new InvalidAttributeException("The patient attribute 'email' can't be empty!");

    if(patient.Birthdate != null)
    {
      var dtbd = patient.Birthdate?.ToDateTime(TimeOnly.MinValue);

      if(DateTime.Now.CompareTo(dtbd) < 0)
      {
        throw new InvalidAttributeException(
            "The patient field 'birthdate' can't be a date in the future!");
      }
    }

    if(patient.PhoneNumber != null)
    {
      var regex = @"^[0-9]{3}-[0-9]{3}-[0-9]{4}$";
      var match = Regex.Match(patient.PhoneNumber, regex, RegexOptions.IgnoreCase);

      if(!match.Success)
      {
        throw new InvalidAttributeException(
            "The patient attribute 'phoneNumber' has to match the format '123-456-7890'!");
      }
    }

    if(patient.Email != null)
    {
      var regex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
      var match = Regex.Match(patient.Email, regex, RegexOptions.IgnoreCase);

      if(!match.Success)
      {
        throw new InvalidAttributeException(
            "The patient attribute 'email' has to match the format 'patientemail@service.com'!");
      }
    }
  }

  public PatientService(IPatientRepository repository)
  {
    _repository = repository;
  }


  private readonly IPatientRepository _repository;
}