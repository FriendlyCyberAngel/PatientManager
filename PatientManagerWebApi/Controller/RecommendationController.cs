namespace PatientManagerWebAPI.Controller;

using Microsoft.AspNetCore.Mvc;
using PatientManagerWebAPI.Service;
using PatientManagerWebAPI.Model;
using PatientManagerWebAPI.Request;
using System.Text.Encodings.Web;


[ApiController]
[Route("api/recommendation")]
public class RecommendationController : ControllerBase
{
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var recommendation = await _service.GetById(id);
    var response = ConvertModelToResponse(recommendation);
    return Ok(response);
  }

  [HttpGet("by-patient-id/{patientId:guid}/{skip:int}/{amount:int}")]
  public async Task<IActionResult> GetByPatientId(
      Guid patientId, int skip, int amount)
  {
    var recommendationList = await _service.GetByPatientId(patientId, skip, amount);
    var response = ConvertModelListToResponse(recommendationList);
    return Ok(new {list = response});
  }

  [HttpGet("by-patient-id-and-completed/{patientId:guid}/{completed:bool}/{skip:int}/{amount:int}")]
  public async Task<IActionResult> GetByCompleted(
      Guid patientId, bool completed, int skip, int amount)
  {
    var list = await _service.GetByPatientIdAndCompleted(
        patientId, completed, skip, amount);
    var response = ConvertModelListToResponse(list);
    return Ok(new {list = response});
  }

  [HttpPost()]
  public async Task<IActionResult> Create(CreateRecommendationRequest request)
  {
    var recommendation = ConvertRequestToModel(request);
    var newRecommendation = await _service.Add(recommendation);
    var response = ConvertModelToResponse(newRecommendation);
    return Ok(response);
  }

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, UpdateRecommendationRequest request)
  {
    var recommendation = ConvertRequestToModel(id, request);
    var updatedRecommendation = await _service.Update(recommendation);
    var response = ConvertModelToResponse(updatedRecommendation);
    return Ok(response);
  }

  [HttpPut("completed/{id:guid}")]
  public async Task<IActionResult> UpdateCompleted(
      Guid id, UpdateRecommendationCompleteRequest request)
  {
    var recommendation = ConvertRequestToModel(id, request);
    var updatedRecommendation = await _service.Update(recommendation);
    var response = ConvertModelToResponse(updatedRecommendation);
    return Ok(response);
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    var recommendation = await _service.Remove(id);
    return Ok(recommendation);
  }

  private Recommendation ConvertRequestToModel(CreateRecommendationRequest request)
  {
    return new Recommendation(
      Guid.NewGuid(),
      request.PatientId,
      PreventXSS(request.Description),
      request.Completed,
      DateTime.Now
    );
  }

  private Recommendation ConvertRequestToModel(Guid id, UpdateRecommendationRequest request)
  {
    return new Recommendation(
      id,
      null,
      PreventXSS(request.Description),
      request.Completed,
      null
    );
  }

  private Recommendation ConvertRequestToModel(
      Guid id, UpdateRecommendationCompleteRequest request)
  {
    return new Recommendation(
      id,
      null,
      null,
      request.Completed,
      null
    );
  }

  private RecommendationResponse ConvertModelToResponse(Recommendation? recommendation)
  {
    return new RecommendationResponse(
      recommendation?.Id,
      recommendation?.PatientId,
      PreventXSS(recommendation?.Description),
      recommendation?.Completed,
      recommendation?.CreatedAt
    );
  }

  private IEnumerable<RecommendationResponse>
      ConvertModelListToResponse(IEnumerable<Recommendation> recommendationList)
  {
    var responseList = new List<RecommendationResponse>();

    foreach(var recommendation in recommendationList)
    {
      responseList.Add(new RecommendationResponse(
          recommendation.Id, recommendation.PatientId,
          PreventXSS(recommendation.Description), recommendation.Completed,
          recommendation.CreatedAt
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

  public RecommendationController(
      HtmlEncoder htmlEncoder,
      JavaScriptEncoder javascriptEncoder,
      UrlEncoder urlEncoder,
      IRecommendationService recommendationService)
  {
    _htmlEncoder = htmlEncoder;
    _javascriptEncoder = javascriptEncoder;
    _urlEncoder = urlEncoder;
    _service = recommendationService;
  }


  private readonly HtmlEncoder _htmlEncoder;
  private readonly JavaScriptEncoder _javascriptEncoder;
  private readonly UrlEncoder _urlEncoder;
  private readonly IRecommendationService _service;
}
