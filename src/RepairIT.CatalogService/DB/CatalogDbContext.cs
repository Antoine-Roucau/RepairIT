using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Entite;

namespace RepairIT.CatalogService.DB;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Ordinateur> Ordinateurs => Set<Ordinateur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ordinateur = modelBuilder.Entity<Ordinateur>();

        ordinateur.Property(o => o.Marque).HasMaxLength(100).IsRequired();
        ordinateur.Property(o => o.Modele).HasMaxLength(100).IsRequired();
        ordinateur.Property(o => o.Processeur).HasMaxLength(100).IsRequired();
        ordinateur.Property(o => o.Prix).HasPrecision(10, 2);

        // enum en string dans la BD (au lieu de int)
        ordinateur.Property(o => o.Type).HasConversion<string>().HasMaxLength(20);
    }
}