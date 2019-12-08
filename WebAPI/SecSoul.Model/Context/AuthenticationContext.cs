using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecSoul.Model.Models;

namespace SecSoul.Model.Context
{
    public class AuthenticationContext : IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options):base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
