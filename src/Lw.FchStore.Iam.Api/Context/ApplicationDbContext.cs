using Lw.FchStore.Iam.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lw.FchStore.Iam.Api.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,  int>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}