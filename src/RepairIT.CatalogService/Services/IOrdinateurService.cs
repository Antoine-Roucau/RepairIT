using RepairIT.Shared;

namespace RepairIT.CatalogService.Services;

public interface IOrdinateurService
{
    Task<IReadOnlyList<OrdinateurDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OrdinateurDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<OrdinateurDto> CreateAsync(OrdinateurRequest request, CancellationToken cancellationToken = default);

    // Renvoient false si l'ordinateur n'existe pas
    Task<bool> UpdateAsync(int id, OrdinateurRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}