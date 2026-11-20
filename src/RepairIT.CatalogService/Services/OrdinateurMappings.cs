using RepairIT.CatalogService.Entite;
using RepairIT.Shared;

namespace RepairIT.CatalogService.Services;

// conversions entre Ordinateur (entite) et OrdinateurDto (DTO)
internal static class OrdinateurMappings
{
    public static OrdinateurDto ToDto(this Ordinateur o) =>
        new(o.Id, o.Marque, o.Modele, o.Processeur, o.RamGo, o.StockageGo, o.Type, o.Prix, o.QuantiteStock, o.DateAjout);

    public static Ordinateur ToEntity(this OrdinateurRequest r) => new()
    {
        Marque = r.Marque,
        Modele = r.Modele,
        Processeur = r.Processeur,
        RamGo = r.RamGo,
        StockageGo = r.StockageGo,
        Type = r.Type,
        Prix = r.Prix,
        QuantiteStock = r.QuantiteStock
    };

    public static void AppliquerModifications(this Ordinateur o, OrdinateurRequest r)
    {
        o.Marque = r.Marque;
        o.Modele = r.Modele;
        o.Processeur = r.Processeur;
        o.RamGo = r.RamGo;
        o.StockageGo = r.StockageGo;
        o.Type = r.Type;
        o.Prix = r.Prix;
        o.QuantiteStock = r.QuantiteStock;
    }
}