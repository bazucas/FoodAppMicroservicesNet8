using Tasko.Services.AuthApi.Models;

namespace Tasko.Services.AuthApi.Service.IService;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
}
