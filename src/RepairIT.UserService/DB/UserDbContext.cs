using Microsoft.EntityFrameworkCore;
using RepairIT.UserService.Entite;

namespace RepairIT.UserService.DB;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var utilisateur = modelBuilder.Entity<Utilisateur>();

        utilisateur.Property(u => u.KeycloakUserId).HasMaxLength(64).IsRequired();
        utilisateur.Property(u => u.Nom).HasMaxLength(100).IsRequired();
        utilisateur.Property(u => u.Prenom).HasMaxLength(100).IsRequired();
        utilisateur.Property(u => u.Email).HasMaxLength(200).IsRequired();
        utilisateur.HasIndex(u => u.Email).IsUnique();

        // Le role est en string dans la DB
        utilisateur.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
    }
}