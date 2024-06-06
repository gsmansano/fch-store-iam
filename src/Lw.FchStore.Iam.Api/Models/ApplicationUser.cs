using Microsoft.AspNetCore.Identity;

namespace Lw.FchStore.Iam.Api.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string Fullname { get; set; }
    public bool IsActive { get; set; }
    public int Profile { get; set; }
}

public class ApplicationRole : IdentityRole<int>
{
}