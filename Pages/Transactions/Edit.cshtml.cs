using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDCxC.Data;
using CRUDCxC.Entities;
using Microsoft.OpenApi.Extensions;

namespace CRUDCxC.Pages.Transactions
{
    public class EditModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public EditModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Transaction Transaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["Title"] = "Editar Transacción";

            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Client) // Incluimos el cliente
                .FirstOrDefaultAsync(m => m.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            Transaction = transaction;

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", Transaction.ClientId);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Description", Transaction.DocumentTypeId);
            ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                .Cast<MovementType>()
                .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber", Transaction.ClientId);
                ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "AccountingAccount", Transaction.DocumentTypeId);
                ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                    .Cast<MovementType>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name", Transaction.MovementType);
                return Page();
            }

            var client = await _context.Clients.FindAsync(Transaction.ClientId);
            if (client == null)
            {
                ModelState.AddModelError("", "El cliente seleccionado no existe.");
                ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber", Transaction.ClientId);
                ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "AccountingAccount", Transaction.DocumentTypeId);
                ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                    .Cast<MovementType>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name", Transaction.MovementType);
                return Page();
            }

            if (client.Status == Status.Inactive)
            {
                ModelState.AddModelError("Transaction.ClientId", "No se pueden realizar transacciones para clientes inactivos.");
                ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber", Transaction.ClientId);
                ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "AccountingAccount", Transaction.DocumentTypeId);
                ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                    .Cast<MovementType>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name", Transaction.MovementType);
                return Page();
            }

            if (Transaction.Amount > client.CreditLimit)
            {
                ModelState.AddModelError("Transaction.Amount", "El monto de la transacción no puede ser mayor al límite de crédito del cliente.");
                ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber", Transaction.ClientId);
                ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "AccountingAccount", Transaction.DocumentTypeId);
                ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                    .Cast<MovementType>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name", Transaction.MovementType);
                return Page();
            }

            _context.Attach(Transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(Transaction.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
