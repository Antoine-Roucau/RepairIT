using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Entite;
using RepairIT.Shared;

namespace RepairIT.CatalogService.DB;

// creation et init de la BD du catalogue
public static class CatalogDbInitializer
{
    public static async Task InitialiserAsync(CatalogDbContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Ordinateurs.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Catalogue déjà initialisé, aucune donnée ajoutée");
            return;
        }

        var ordinateurs = OrdinateursDeDepart();
        context.Ordinateurs.AddRange(ordinateurs);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Catalogue initialisé avec {Nombre} ordinateurs", ordinateurs.Length);
    }

    // Ordinateurs de depart pour la BD
    private static Ordinateur[] OrdinateursDeDepart() =>
    [
        new() { Marque = "Dell", Modele = "XPS 13", Processeur = "Intel Core Ultra 7", RamGo = 16, StockageGo = 512, Type = TypeOrdinateur.Portable, Prix = 1299.99m, QuantiteStock = 5 },
        new() { Marque = "Lenovo", Modele = "ThinkPad T14", Processeur = "AMD Ryzen 7 PRO", RamGo = 32, StockageGo = 1024, Type = TypeOrdinateur.Portable, Prix = 1499.00m, QuantiteStock = 3 },
        new() { Marque = "HP", Modele = "EliteDesk 800", Processeur = "Intel Core i5", RamGo = 16, StockageGo = 512, Type = TypeOrdinateur.Fixe, Prix = 849.00m, QuantiteStock = 8 },
        new() { Marque = "Apple", Modele = "iMac 24", Processeur = "Apple M4", RamGo = 16, StockageGo = 256, Type = TypeOrdinateur.ToutEnUn, Prix = 1599.00m, QuantiteStock = 2 },
        new() { Marque = "Asus", Modele = "VivoBook 15", Processeur = "Intel Core i3", RamGo = 8, StockageGo = 256, Type = TypeOrdinateur.Portable, Prix = 499.99m, QuantiteStock = 10 }
    ];
}