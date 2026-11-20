using RepairIT.Shared;

namespace RepairIT.UserService.Entite;

// Entite Utilisateur
// pas de mot de passe car on utilise Keycloak pour l'authentification
public class Utilisateur
{
    // Valeur par defaut tant que l'utilisateur n'est pas relie à Keycloak
    public const string KeycloakUserIdNonSynchronise = "non-synchronise";

    public int Id { get; set; }
    public string KeycloakUserId { get; set; } = KeycloakUserIdNonSynchronise;
    public required string Nom { get; set; }
    public required string Prenom { get; set; }
    public required string Email { get; set; }
    public RoleUtilisateur Role { get; set; } = RoleUtilisateur.Gestionnaire;
    public bool EstActif { get; set; } = true;
}