using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NotSoSmartHome.Persistence;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options)
{
}