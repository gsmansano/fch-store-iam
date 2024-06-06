using System.Security.Claims;
using Lw.FchStore.Iam.Api.Models;
using Lw.FchStore.Iam.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lw.FchStore.Iam.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(ILogger<UsersController> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(model);

        ApplicationUser user = new()
        {
            UserName = model.Email,
            Email = model.Email,
            Fullname = model.Fullname,
            IsActive = true,
            Profile = 1
        };

        var pwd = model.Password;

        var result = await _userManager.CreateAsync(user, pwd);

        if (result.Succeeded)
        {
            await _userManager.AddClaimAsync(user, new Claim("userId", user.Id.ToString()));
        }

        return Ok();
    }
}