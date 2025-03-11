using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUDCxC.Entities;

public class AccountEntry
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DisplayName("Descripción")]
    public string? Description { get; set; }
    [Required]
    [DisplayName("Cliente")]
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    [Required]
    [DisplayName("Número de cuenta")]
    public string? AccountNumber { get; set; }
    [Required]
    [DisplayName("Tipo de movimiento")]
    public MovementType MovementType { get; set; }
    [Required]
    [DisplayName("Fecha Asiento")]
    public DateTime EntryDate { get; set; } = DateTime.UtcNow;
    [Required]
    [DisplayName("Monto Asiento")]
    public decimal Amount { get; set; }
    [Required]
    [DisplayName("Estado")]
    public Status Status { get; set; }
}