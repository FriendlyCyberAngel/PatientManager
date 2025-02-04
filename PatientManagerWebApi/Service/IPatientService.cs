namespace PatientManagerWebAPI.Service;

using PatientManagerWebAPI.Model;


public interface IPatientService
{
  Task<IEnumerable<Patient>> GetAll(int skip, int amount);
  Task<IEnumerable<Patient>> GetByName(string name, int skip, int amount);

  Task<Patient?> GetById(Guid id);
  Task<Patient?> Add(Patient patient);
  Task<Patient?> Update(Patient patient);
  Task<Patient?> Remove(Guid id);
}