using Microsoft.EntityFrameworkCore;
using RepairIT.Shared;
using RepairIT.UserService.Entite;

namespace RepairIT.UserService.DB;

// creation et init de la base
public static class UserDbInitializer
{
    public static async Task InitialiserAsync(UserDbContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Utilisateurs.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Utilisateurs déjà initialisés, aucune donnée ajoutée");
            return;
        }

        var utilisateurs = UtilisateursDeDepart();
        context.Utilisateurs.AddRange(utilisateurs);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Base utilisateurs initialisée avec {Nombre} utilisateurs", utilisateurs.Length);
    }

    // Utilisateurs de depart
    private static Utilisateur[] UtilisateursDeDepart() =>
    [
        new() { Nom = "Admin", Prenom = "Alice", Email = "admin1@email.com", Role = RoleUtilisateur.Administrateur },
        new() { Nom = "RepairIT", Prenom = "Gestionnaire1", Email = "gestionnaire1@email.com" },
        new() { Nom = "RepairIT", Prenom = "Gestionnaire2", Email = "gestionnaire2@email.com" },
        new() { Nom = "RepairIT", Prenom = "Gestionnaire3", Email = "gestionnaire3@email.com" },
        new() { Nom = "RepairIT", Prenom = "Gestionnaire4", Email = "gestionnaire4@email.com" }
    ];
}