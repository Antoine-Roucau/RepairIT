namespace RepairIT.Shared;

// Ordinateur renvoye par l'API
public sealed record OrdinateurDto(
    int Id,
    string Marque,
    string Modele,
    string Processeur,
    int RamGo,
    int StockageGo,
    TypeOrdinateur Type,
    decimal Prix,
    int QuantiteStock,
    DateTime DateAjout);