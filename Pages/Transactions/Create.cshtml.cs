using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ViewData["Title"] = "Crear Transacción";

            ViewData["ClientId"] = new SelectList(
                _context.Clients
                    .Where(c => c.IdentificationNumber != "00000000000")
                    .ToList(),
                "Id",
                "Name"
            );

            ViewData["DocumentTypeId"] = new SelectList(
                _context.DocumentTypes
                    .Where(dt => dt.Description != "Ingresos x Ventas")
                    .ToList(),
                "Id",
                "Description"
            );

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
            ViewData["Title"] = "Crear Transacción";

            if (!ModelState.IsValid)
            {
                ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                    .Cast<MovementType>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");

                ViewData["ClientId"] = new SelectList(
                    _context.Clients
                        .Where(c => c.IdentificationNumber != "00000000000")
                        .ToList(),
                    "Id",
                    "Name"
                );

                ViewData["DocumentTypeId"] = new SelectList(
                    _context.DocumentTypes
                        .Where(dt => dt.Description != "Ingresos x Ventas")
                        .ToList(),
                    "Id",
                    "Description"
                );

                return Page();
            }

            var client = await _context.Clients.FindAsync(Transaction.ClientId);

            if (client == null)
            {
                ModelState.AddModelError("", "El cliente seleccionado no existe.");
                return Page();
            }

            if (client.IdentificationNumber == "00000000000")
            {
                ModelState.AddModelError("Transaction.ClientId", "El cliente no es válido.");
                return Page();
            }

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

            int lastId = await _context.Transactions.MaxAsync(t => (int?)t.Id) ?? 0;

            Transaction.DocumentNumber = $"DOC-{lastId + 1:D5}";
            Transaction.Date = DateTime.Now;
            Transaction.MovementType = MovementType.Debit;

            _context.Transactions.Add(Transaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
