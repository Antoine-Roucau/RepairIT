namespace RepairIT.Shared;

// Utilisateur renvoye par l'API
public sealed record UtilisateurDto(
    int Id,
    string KeycloakUserId,
    string Nom,
    string Prenom,
    string Email,
    RoleUtilisateur Role,
    bool EstActif);