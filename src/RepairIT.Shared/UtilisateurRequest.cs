using System.ComponentModel.DataAnnotations;

namespace RepairIT.Shared;

// donnees envoyees pour creer un utilisateur (pas de mot de passe, gere par Keycloak)
public sealed record UtilisateurRequest(
    [Required, StringLength(100)] string Nom,
    [Required, StringLength(100)] string Prenom,
    [Required, EmailAddress, StringLength(200)] string Email,
    RoleUtilisateur Role);