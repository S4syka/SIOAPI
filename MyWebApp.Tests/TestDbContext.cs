using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace MyWebApp.Tests;

// Derived context that skips Npgsql configuration
public class TestDbContext : OaDbContext
{
    public TestDbContext(DbContextOptions<OaDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Intentionally left blank to avoid PostgreSQL configuration
    }
}
