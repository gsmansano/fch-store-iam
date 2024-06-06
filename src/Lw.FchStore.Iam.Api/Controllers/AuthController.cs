using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lw.FchStore.Iam.Api.Models;
using Lw.FchStore.Iam.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TimeZoneConverter;

namespace Lw.FchStore.Iam.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(ILogger<AuthController> logger, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            if (model is null || string.IsNullOrWhiteSpace(model.Email))
            {
                _logger.LogCritical("Someone tried to login: " + model.Email);
                return BadRequest(new
                {
                    message = "Object required"
                });
            }

            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                _logger.LogCritical("Someone was unauthorized to login: " + model.Email);
                return Unauthorized(new
                {
                    authenticated = false,
                    message = "Email ou Senha inválido"
                });
            }

            var userData = await _userManager.FindByNameAsync(model.Email);

            _logger.LogInformation("Someone logged: " + model.Email);

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("Fullname", userData.Fullname),
                new Claim("Email", userData.Email),
                new Claim("UserId", userData.Id.ToString()),
                new Claim("Profile", userData.Profile.ToString())
            });

            var expireDate = DateTime.UtcNow.AddHours(3);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                Subject = claimsIdentity,
                Expires = expireDate,
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ABCC70A3-D642-4D56-8985-019DF837BC9D")),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenValue = tokenHandler.WriteToken(createdToken);

            expireDate = TimeZoneInfo.ConvertTime(expireDate, TZConvert.GetTimeZoneInfo("GMT Standard Time"));

            return Ok(new
            {
                @token_type = "Bearer",
                @expires_in = expireDate.ToString("yyyy-MM-dd HH:mm:ss"),
                @access_token = tokenValue
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}