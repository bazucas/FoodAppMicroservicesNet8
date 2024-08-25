using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tasko.Web.Models;
using Tasko.Web.Service.IService;
using Tasko.Web.Utility;

namespace Tasko.Web.Controllers;

public class AuthController(IAuthService authService, ITokenProvider tokenProvider) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDto loginRequestDto = new();
        return View(loginRequestDto);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto obj)
    {
        ResponseDto responseDto = await authService.LoginAsync(obj);

        if (responseDto != null && responseDto.IsSuccess)
        {
            LoginResponseDto loginResponseDto =
                JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

            await SignInUser(loginResponseDto);
            tokenProvider.SetToken(loginResponseDto.Token);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            TempData["error"] = responseDto.Message;
            return View(obj);
        }
    }


    [HttpGet]
    public IActionResult Register()
    {
        var roleList = new List<SelectListItem>()
        {
            new() {Text=SD.RoleAdmin,Value=SD.RoleAdmin},
            new() {Text=SD.RoleCustomer,Value=SD.RoleCustomer},
        };

        ViewBag.RoleList = roleList;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationRequestDto obj)
    {
        ResponseDto result = await authService.RegisterAsync(obj);
        ResponseDto assingRole;

        if (result != null && result.IsSuccess)
        {
            if (string.IsNullOrEmpty(obj.Role))
            {
                obj.Role = SD.RoleCustomer;
            }
            assingRole = await authService.AssignRoleAsync(obj);
            if (assingRole != null && assingRole.IsSuccess)
            {
                TempData["success"] = "Registration Successful";
                return RedirectToAction(nameof(Login));
            }
        }
        else
        {
            TempData["error"] = result.Message;
        }

        var roleList = new List<SelectListItem>()
        {
            new() {Text=SD.RoleAdmin,Value=SD.RoleAdmin},
            new() {Text=SD.RoleCustomer,Value=SD.RoleCustomer},
        };

        ViewBag.RoleList = roleList;
        return View(obj);
    }


    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        tokenProvider.ClearToken();
        return RedirectToAction("Index", "Home");
    }


    private async Task SignInUser(LoginResponseDto model)
    {
        var handler = new JwtSecurityTokenHandler();

        var jwt = handler.ReadJwtToken(model.Token);

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


        identity.AddClaim(new Claim(ClaimTypes.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
        identity.AddClaim(new Claim(ClaimTypes.Role,
            jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));



        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

}
