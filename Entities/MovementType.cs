using System.ComponentModel.DataAnnotations;

namespace CRUDCxC.Entities;

public enum MovementType
{
    [Display(Name = "Débito")]
    Debit, //DB
    [Display(Name = "Crédito")]
    Credit //CR
}