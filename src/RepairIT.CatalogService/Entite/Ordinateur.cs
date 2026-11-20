using RepairIT.Shared;

namespace RepairIT.CatalogService.Entite;

// Entite Ordinateur
public class Ordinateur
{
    public int Id { get; set; }
    public required string Marque { get; set; }
    public required string Modele { get; set; }
    public required string Processeur { get; set; }
    public int RamGo { get; set; }
    public int StockageGo { get; set; }
    public TypeOrdinateur Type { get; set; }
    public decimal Prix { get; set; }
    public int QuantiteStock { get; set; }
    public DateTime DateAjout { get; set; } = DateTime.UtcNow;
}