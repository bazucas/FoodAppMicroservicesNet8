using Microsoft.AspNetCore.Identity;

namespace Tasko.Services.AuthApi.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}
