using CRUDCxC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CRUDCxC.Pages.AccountingEntry;

public class CreateModel : PageModel
{
    [BindProperty]
    public DateTime? FechaDesde { get; set; }

    [BindProperty]
    public DateTime? FechaHasta { get; set; }
    public List<Transaction> TransaccionesFiltradas { get; set; } = new();
    private readonly CRUDCxC.Data.CxCDbContext _context;

    public CreateModel(CRUDCxC.Data.CxCDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        ViewData["Title"] = "Crear Asiento Contable";
    }

    public async Task<IActionResult> OnPost(string? action)
    {
        ViewData["Title"] = "Crear Asiento Contable";

        if (!FechaDesde.HasValue || !FechaHasta.HasValue)
        {
            ModelState.AddModelError(string.Empty, "Debe seleccionar ambas fechas.");
            return Page();
        }

        if (FechaDesde > FechaHasta)
        {
            ModelState.AddModelError(string.Empty, "La fecha 'desde' no puede ser mayor que la fecha 'hasta'.");
            return Page();
        }

        TransaccionesFiltradas = await _context.Transactions
        .Include(t => t.Client)
        .Include(t => t.DocumentType)
        .Where(t => t.Date.Date >= FechaDesde.Value.Date && t.Date.Date <= FechaHasta.Value.Date)
        .OrderBy(t => t.Date)
        .ToListAsync();

        if (action != "contabilizar")
        {
            return Page();
        }

        var ingresosDoc = await _context.DocumentTypes
    .FirstOrDefaultAsync(dt => dt.Description == "Ingresos x Ventas");

        if (ingresosDoc == null)
        {
            ModelState.AddModelError(string.Empty, "No se encontró el documento 'Ingresos x Ventas'.");
            return Page();
        }

        var total = TransaccionesFiltradas.Sum(t => t.Amount);
        Console.WriteLine($"🧾 Total contabilizado: {total:C}");

        int siguienteDocNum = await _context.Transactions.CountAsync() + 1;
        string numeroDocumento = $"DOC-{siguienteDocNum:D5}";

        var clienteContable = await _context.Clients
            .FirstOrDefaultAsync(c => c.IdentificationNumber == "00000000000");

        if (clienteContable == null)
        {
            clienteContable = new Client
            {
                Name = "Cliente Contable",
                IdentificationNumber = "000-0000000-0",
                CreditLimit = decimal.MaxValue,
                Status = Status.Active
            };

            _context.Clients.Add(clienteContable);
            await _context.SaveChangesAsync();
        }


        var transaccionResumen = new Transaction
        {
            Amount = total,
            Date = DateTime.Now,
            ClientId = clienteContable.Id,
            DocumentTypeId = ingresosDoc.Id,
            MovementType = MovementType.Credit,
            DocumentNumber = numeroDocumento,

        };

        try
        {
            _context.Transactions.Add(transaccionResumen);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Se contabilizó correctamente por un total de {total:C}");
            TempData["Success"] = $"Se contabilizó correctamente por un total de {total:C}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al guardar la transacción resumen: {ex.Message}");
            ModelState.AddModelError(string.Empty, "Hubo un error al guardar la transacción contable. Intente nuevamente.");
        }

        return Page();
    }
}
