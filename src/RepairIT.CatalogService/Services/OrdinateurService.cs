using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.DB;
using RepairIT.Shared;

namespace RepairIT.CatalogService.Services;

public sealed class OrdinateurService(CatalogDbContext context, ILogger<OrdinateurService> logger) : IOrdinateurService
{
    public async Task<IReadOnlyList<OrdinateurDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Ordinateurs
            .AsNoTracking()
            .OrderBy(o => o.Marque)
            .ThenBy(o => o.Modele)
            .Select(o => o.ToDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<OrdinateurDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var ordinateur = await context.Ordinateurs
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        return ordinateur?.ToDto();
    }

    public async Task<OrdinateurDto> CreateAsync(OrdinateurRequest request, CancellationToken cancellationToken = default)
    {
        var ordinateur = request.ToEntity();
        context.Ordinateurs.Add(ordinateur);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Ordinateur {Id} créé : {Marque} {Modele}", ordinateur.Id, ordinateur.Marque, ordinateur.Modele);
        return ordinateur.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, OrdinateurRequest request, CancellationToken cancellationToken = default)
    {
        var ordinateur = await context.Ordinateurs.FindAsync([id], cancellationToken);
        if (ordinateur is null)
        {
            logger.LogWarning("Modification impossible : ordinateur {Id} introuvable", id);
            return false;
        }

        ordinateur.AppliquerModifications(request);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Ordinateur {Id} modifié", id);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var ordinateur = await context.Ordinateurs.FindAsync([id], cancellationToken);
        if (ordinateur is null)
        {
            logger.LogWarning("Suppression impossible : ordinateur {Id} introuvable", id);
            return false;
        }

        context.Ordinateurs.Remove(ordinateur);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Ordinateur {Id} supprimé", id);
        return true;
    }
}