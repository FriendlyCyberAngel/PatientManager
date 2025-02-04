namespace PatientManagerWebAPI.Controller;

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using PatientManagerWebAPI.Service;
using PatientManagerWebAPI.Model;
using PatientManagerWebAPI.Request;


[ApiController]
[Route("api/patient")]
public class PatientController : ControllerBase
{
  [HttpGet("{skip:int}/{amount:int}")]
  public async Task<IActionResult> GetAll(int skip, int amount)
  {
    var patientList = await _service.GetAll(skip, amount);
    var response = ConvertModelListToResponse(patientList);
    return Ok(new {list = response});
  }

  [HttpGet("by-name/{name}/{skip:int}/{amount:int}")]
  public async Task<IActionResult> GetByName(string name, int skip, int amount)
  {
    var patientList = await _service.GetByName(name, skip, amount);
    var response = ConvertModelListToResponse(patientList);
    return Ok(new {list = response});
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var patient = await _service.GetById(id);
    var response = ConvertModelToResponse(patient);
    return Ok(response);
  }

  [HttpPost()]
  public async Task<IActionResult> Create(CreatePatientRequest request)
  {
    var patient = ConvertRequestToModel(request);
    var newPatient = await _service.Add(patient);
    var response = ConvertModelToResponse(newPatient);
    return Ok(response);
  }

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, UpdatePatientRequest request)
  {
    var patient = ConvertRequestToModel(id, request);
    var updatedPatient = await _service.Update(patient);
    var response = ConvertModelToResponse(updatedPatient);
    return Ok(response);
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    var patient = await _service.Remove(id);
    return Ok(patient);
  }

  private Patient ConvertRequestToModel(CreatePatientRequest request)
  {
    return new Patient(
      Guid.NewGuid(),
      PreventXSS(request.Name),
      request.Birthdate,
      PreventXSS(request.PhoneNumber),
      PreventXSS(request.Email),
      DateTime.Now
    );
  }

  private Patient ConvertRequestToModel(Guid id, UpdatePatientRequest request)
  {
    return new Patient(
      id,
      PreventXSS(request.Name),
      request.Birthdate,
      PreventXSS(request.PhoneNumber),
      PreventXSS(request.Email),
      null
    );
  }

  private PatientResponse ConvertModelToResponse(Patient? patient)
  {
    return new PatientResponse(
      patient?.Id,
      PreventXSS(patient?.Name),
      patient?.Birthdate,
      PreventXSS(patient?.PhoneNumber),
      PreventXSS(patient?.Email),
      patient?.CreatedAt
    );
  }

  private IEnumerable<PatientResponse>
      ConvertModelListToResponse(IEnumerable<Patient> patientList)
  {
    var responseList = new List<PatientResponse>();

    foreach(var patient in patientList)
    {
      responseList.Add(new PatientResponse(
          patient.Id, PreventXSS(patient.Name), patient.Birthdate,
          PreventXSS(patient.PhoneNumber), PreventXSS(patient.Email),
          patient.CreatedAt
      ));
    }

    return responseList;
  }

  private string? PreventXSS(string? data)
  {
    if(data != null)
    {
      var encoded = _htmlEncoder.Encode(data);
      return _javascriptEncoder.Encode(encoded);
    }

    return null;
  }

  public PatientController(
      HtmlEncoder htmlEncoder,
      JavaScriptEncoder javascriptEncoder,
      UrlEncoder urlEncoder,
      IPatientService patientService)
  {
    _htmlEncoder = htmlEncoder;
    _javascriptEncoder = javascriptEncoder;
    _urlEncoder = urlEncoder;
    _service = patientService;
  }


  private readonly HtmlEncoder _htmlEncoder;
  private readonly JavaScriptEncoder _javascriptEncoder;
  private readonly UrlEncoder _urlEncoder;
  private readonly IPatientService _service;
}
