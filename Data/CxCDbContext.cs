using CRUDCxC.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CRUDCxC.Data;

public class CxCDbContext : DbContext
{
    public CxCDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Client> Clients { get; set; }
    public DbSet<AccountEntry> AccountEntries { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Establecer precisión para montos
        modelBuilder.Entity<Client>()
            .Property(c => c.CreditLimit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<AccountEntry>()
            .Property(a => a.Amount)
            .HasPrecision(18, 2);

        base.OnModelCreating(modelBuilder);
    }

}
