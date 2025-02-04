namespace PatientManagerWebAPI.Controller;

using Microsoft.AspNetCore.Mvc;
using PatientManagerWebAPI.Service;
using PatientManagerWebAPI.Model;
using PatientManagerWebAPI.Request;
using System.Text.Encodings.Web;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
  [HttpGet("{skip:int}/{amount:int}")]
  public async Task<IActionResult> GetAll(int skip, int amount)
  {
    var userList = await _service.GetAll(skip, amount);
    var response = ConvertModelListToResponse(userList);
    return Ok(new {list = response});
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
  {
    var user = await _service.GetById(id);
    var response = ConvertModelToResponse(user);
    return Ok(response);
  }

  [HttpPost()]
  public async Task<IActionResult> Create(CreateUserRequest request)
  {
    var user = ConvertRequestToModel(request);
    var newUser = await _service.Add(user);
    var response = ConvertModelToResponse(newUser);
    return Ok(response);
  }

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
  {
    var user = ConvertRequestToModel(id, request);
    var updatedUser = await _service.Update(user);
    var response = ConvertModelToResponse(updatedUser);
    return Ok(response);
  }

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    var user = await _service.Remove(id);
    return Ok(user);
  }

  private User ConvertRequestToModel(CreateUserRequest request)
  {
    return new User(
      Guid.NewGuid(),
      PreventXSS(request.Name),
      PreventXSS(request.PhoneNumber),
      PreventXSS(request.Email),
      request.Role,
      PreventXSS(request.Login),
      (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString("X"),
      DateTime.Now
    );
  }

  private User ConvertRequestToModel(Guid id, UpdateUserRequest request)
  {
    return new User(
      id,
      PreventXSS(request.Name?.Replace(" ", "")),
      PreventXSS(request.PhoneNumber),
      PreventXSS(request.Email),
      request.Role,
      null,
      null,
      null
    );
  }

  private UserResponse ConvertModelToResponse(User? user)
  {
    return new UserResponse(
      user?.Id,
      PreventXSS(user?.Name),
      PreventXSS(user?.PhoneNumber),
      PreventXSS(user?.Email),
      user?.Role,
      user?.CreatedAt
    );
  }

  private IEnumerable<UserResponse> ConvertModelListToResponse(IEnumerable<User> userList)
  {
    var responseList = new List<UserResponse>();

    foreach(var user in userList)
    {
      responseList.Add(new UserResponse(
          user.Id, PreventXSS(user.Name), PreventXSS(user.PhoneNumber),
          PreventXSS(user.Email), user.Role, user.CreatedAt
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

  public UserController(
      HtmlEncoder htmlEncoder,
      JavaScriptEncoder javascriptEncoder,
      UrlEncoder urlEncoder,
      IUserService userService)
  {
    _htmlEncoder = htmlEncoder;
    _javascriptEncoder = javascriptEncoder;
    _urlEncoder = urlEncoder;
    _service = userService;
  }


  private readonly HtmlEncoder _htmlEncoder;
  private readonly JavaScriptEncoder _javascriptEncoder;
  private readonly UrlEncoder _urlEncoder;
  private readonly IUserService _service;
}
