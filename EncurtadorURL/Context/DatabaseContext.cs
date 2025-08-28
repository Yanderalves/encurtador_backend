using EncurtadorURL.Models;
using Microsoft.EntityFrameworkCore;

namespace EncurtadorURL.Context;

public class DatabaseContext : DbContext
{
    public DbSet<URLModel> URLs { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<URLModel>()
            .HasKey(x => x.Id);
    }
}