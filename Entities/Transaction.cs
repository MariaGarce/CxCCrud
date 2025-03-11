using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUDCxC.Entities;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DisplayName("Tipo de Movimiento")]
    public MovementType MovementType { get; set; }

    [Required]
    [DisplayName("Tipo de Documento")]
    public int DocumentTypeId { get; set; }
    public DocumentType? DocumentType { get; set; }

    [Required]
    [DisplayName("Numero del documento")]
    public string? DocumentNumber { get; set; }

    [Required]
    [DisplayName("Fecha")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [DisplayName("Cliente")]
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    [Required]
    [DisplayName("Monto")]
    public decimal Amount { get; set; }
}