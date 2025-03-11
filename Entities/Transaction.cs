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
    [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
    public decimal Amount { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var dbContext = (CRUDCxC.Data.CxCDbContext)validationContext.GetService(typeof(CRUDCxC.Data.CxCDbContext));
        var client = dbContext.Clients.Find(ClientId);

        if (client != null && Amount > client.CreditLimit)
        {
            yield return new ValidationResult("El monto no puede ser mayor al límite de crédito del cliente.", new[] { "Amount" });
        }
    }
}