using System.ComponentModel.DataAnnotations;

namespace CRUDCxC.Entities;

public enum Status
{
    [Display(Name = "Activo")]
    Active,
    [Display(Name = "Inactivo")]
    Inactive
}