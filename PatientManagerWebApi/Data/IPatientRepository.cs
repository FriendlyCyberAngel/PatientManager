namespace PatientManagerWebAPI.Data;

using PatientManagerWebAPI.Model;


public interface IPatientRepository
{
  Task<IEnumerable<Patient>> GetAll(int skip, int amount);
  Task<IEnumerable<Patient>> GetByName(string name, int skip, int amount);
  Task<Patient?> GetById(Guid id);
  Task<Patient?> Insert(Patient patient);
  Task<int> Update(Patient patient);
  Task<int> Delete(Guid id);
}