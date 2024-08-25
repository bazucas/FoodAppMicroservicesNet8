namespace Tasko.Services.AuthApi.Models.Dto;

public class LoginResponseDto
{
    public required UserDto User { get; set; }
    public string Token { get; set; } = string.Empty;
}
