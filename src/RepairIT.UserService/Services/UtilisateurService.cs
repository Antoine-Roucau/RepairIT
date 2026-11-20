using Microsoft.EntityFrameworkCore;
using RepairIT.Shared;
using RepairIT.UserService.DB;
using RepairIT.UserService.Entite;

namespace RepairIT.UserService.Services;

public sealed class UtilisateurService(UserDbContext context, ILogger<UtilisateurService> logger, IConfiguration configuration)
    : IUtilisateurService
{
    public async Task<IReadOnlyList<UtilisateurDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Utilisateurs
            .AsNoTracking()
            .OrderBy(u => u.Nom)
            .ThenBy(u => u.Prenom)
            .Select(u => u.ToDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<UtilisateurDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var utilisateur = await context.Utilisateurs
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return utilisateur?.ToDto();
    }

    public async Task<UtilisateurDto?> CreateAsync(UtilisateurRequest request, CancellationToken cancellationToken = default)
    {
        if (await EmailExistsAsync(request.Email, cancellationToken))
        {
            logger.LogWarning("Création impossible : l'email {Email} est déjà utilisé", request.Email);
            return null;
        }

        var utilisateur = request.ToEntity(GetDefaultKeycloakUserId());
        context.Utilisateurs.Add(utilisateur);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Utilisateur {Id} créé : {Prenom} {Nom} ({Role})", utilisateur.Id, utilisateur.Prenom, utilisateur.Nom, utilisateur.Role);
        return utilisateur.ToDto();
    }

    // email compare sans tenir compte des majuscules
    private Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken) =>
        context.Utilisateurs.AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);

    // Keycloak pas encore gere en V1 : valeur par defaut lue dans appsettings.json
    private string GetDefaultKeycloakUserId() =>
        configuration["Keycloak:UserIdParDefaut"] ?? Utilisateur.KeycloakUserIdNonSynchronise;
}