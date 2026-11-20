using RepairIT.Shared;
using RepairIT.UserService.Entite;

namespace RepairIT.UserService.Services;

// conversions entre Utilisateur (entite) et UtilisateurDto (DTO)
internal static class UtilisateurMappings
{
    public static UtilisateurDto ToDto(this Utilisateur u) =>
        new(u.Id, u.KeycloakUserId, u.Nom, u.Prenom, u.Email, u.Role, u.EstActif);

    public static Utilisateur ToEntity(this UtilisateurRequest r, string keycloakUserId) => new()
    {
        KeycloakUserId = keycloakUserId,
        Nom = r.Nom,
        Prenom = r.Prenom,
        Email = r.Email,
        Role = r.Role
    };
}