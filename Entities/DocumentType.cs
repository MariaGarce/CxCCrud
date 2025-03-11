using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUDCxC.Entities;

public class DocumentType
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DisplayName("Descripción")]
    public string? Description { get; set; }
    [Required]
    [DisplayName("Cuenta Contable")]
    public string? AccountingAccount { get; set; }
    [Required]
    [DisplayName("Estado")]
    public Status Status { get; set; }
}