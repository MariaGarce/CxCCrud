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

}
