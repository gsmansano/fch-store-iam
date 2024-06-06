using Microsoft.AspNetCore.Identity;

namespace Lw.FchStore.Iam.Api.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string Fullname { get; set; }
    public bool IsActive { get; set; }
    public ProfileType Profile { get; set; }
}

public class ApplicationRole : IdentityRole<int>
{
}

public enum ProfileType
{
    ADMINSTRATOR = 1000,    
    CLIENT = 1
}

