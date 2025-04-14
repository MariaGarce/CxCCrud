using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDCxC.Pages.AccountingEntry;

public class CreateModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Crear Asiento Contable";
    }
}
