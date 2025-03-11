using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUDCxC.Entities;

public class Client
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DisplayName("Nombre")]
    public string? Name { get; set; }
    [Required]
    [DisplayName("Cédula")]
    public string? IdentificationNumber { get; set; }
    [Required]
    [DisplayName("Límite de Credito")]
    public decimal CreditLimit { get; set; }
    [Required]
    [DisplayName("Estado")]
    public Status Status { get; set; }
}