using System.ComponentModel.DataAnnotations;

namespace RepairIT.Shared;

// donnees envoye pour creer ou modifier un ordinateur
public sealed record OrdinateurRequest(
    [Required, StringLength(100)] string Marque,
    [Required, StringLength(100)] string Modele,
    [Required, StringLength(100)] string Processeur,
    [Range(1, 1024)] int RamGo,
    [Range(1, 16384)] int StockageGo,
    TypeOrdinateur Type,
    [Range(typeof(decimal), "0", "100000")] decimal Prix,
    [Range(0, 10000)] int QuantiteStock);