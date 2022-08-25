using CorridaAPI.Authentication.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CorridaAPI.Data;

public class CorridaContext : IdentityDbContext<ApplicationUser>
{
    public CorridaContext(DbContextOptions<CorridaContext> options) : base(options)
    {

    }
}
