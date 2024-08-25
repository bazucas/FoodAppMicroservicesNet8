using Microsoft.AspNetCore.Mvc;
using Tasko.Integration.MessageBus;
using Tasko.Services.AuthApi.Models.Dto;
using Tasko.Services.AuthApi.Service.IService;

namespace Tasko.Services.AuthApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthApiController(IAuthService authService, IMessageBus messageBus, IConfiguration configuration)
    : ControllerBase
{
    protected new ResponseDto Response = new();


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
    {

        var errorMessage = await authService.Register(model);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            Response.IsSuccess = false;
            Response.Message = errorMessage;
            return BadRequest(Response);
        }
        await messageBus.PublishMessage(model.Email, configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
        return Ok(Response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var loginResponse = await authService.Login(model);

        if (loginResponse?.User is null)
        {
            Response.IsSuccess = false;
            Response.Message = "Username or password is incorrect";
            return BadRequest(Response);
        }

        Response.Result = loginResponse;
        return Ok(Response);

    }

    [HttpPost("AssignRole")]
    public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
    {
        var assignRoleSuccessful = await authService.AssignRole(model.Email, model.Role.ToUpper());
        if (!assignRoleSuccessful)
        {
            Response.IsSuccess = false;
            Response.Message = "Error encountered";
            return BadRequest(Response);
        }
        return Ok(Response);
    }
}
