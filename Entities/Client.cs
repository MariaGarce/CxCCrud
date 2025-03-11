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
    [CedulaDominicana(ErrorMessage = "El número de cédula no es válido.")]
    public string? IdentificationNumber { get; set; }
    [Required]
    [DisplayName("Limite de crédito")]
    [Range(1, double.MaxValue, ErrorMessage = "El límite de crédito debe ser mayor que 0.")]
    public decimal CreditLimit { get; set; }
    [Required]
    [DisplayName("Estado")]
    public Status Status { get; set; }
}