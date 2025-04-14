using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDCxC.Pages.AccountingEntry;

public class CreateModel : PageModel
{
    [BindProperty]
    public DateTime? FechaDesde { get; set; }

    [BindProperty]
    public DateTime? FechaHasta { get; set; }

    public void OnGet()
    {
        ViewData["Title"] = "Crear Asiento Contable";
    }

    public IActionResult OnPost()
    {
        if (!FechaDesde.HasValue || !FechaHasta.HasValue)
        {
            ModelState.AddModelError(string.Empty, "Debe seleccionar ambas fechas.");
            return Page();
        }

        // Próximamente: consultar transacciones en ese rango...

        return Page();
    }
}
