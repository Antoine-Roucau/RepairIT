using RepairIT.Shared;

namespace RepairIT.UserService.Services;

public interface IUtilisateurService
{
    Task<IReadOnlyList<UtilisateurDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UtilisateurDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    // Renvoie null si l'email est deja utilise
    Task<UtilisateurDto?> CreateAsync(UtilisateurRequest request, CancellationToken cancellationToken = default);
}