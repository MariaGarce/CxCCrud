using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CRUDCxC.Data;
using CRUDCxC.Entities;
using Microsoft.OpenApi.Extensions;

namespace CRUDCxC.Pages.Transactions
{
    public class CreateModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public CreateModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber");
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "AccountingAccount");
            ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
        .Cast<MovementType>()
        .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");

            return Page();
        }

        [BindProperty]
        public Transaction Transaction { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Buscar el cliente para obtener su límite de crédito
            var client = await _context.Clients.FindAsync(Transaction.ClientId);

            if (client == null)
            {
                ModelState.AddModelError("", "El cliente seleccionado no existe.");
                return Page();
            }

            // Validar que el monto no supere el límite de crédito
            if (Transaction.Amount > client.CreditLimit)
            {
                ModelState.AddModelError("Transaction.Amount", "El monto no puede ser mayor al límite de crédito del cliente.");
                return Page();
            }
            if (client.Status == Status.Inactive)
            {
                ModelState.AddModelError("Transaction.ClientId", "No se pueden realizar transacciones para clientes inactivos.");
                return Page();
            }

            _context.Transactions.Add(Transaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
