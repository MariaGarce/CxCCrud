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

        private async Task CargarViewDataAsync()
        {
            // Cliente contable
            var isContableClient = await _context.Clients
                .Where(c => c.Id == Transaction.ClientId)
                .Select(c => c.IdentificationNumber == "00000000000")
                .FirstOrDefaultAsync();

            var clientes = isContableClient
                ? _context.Clients.Where(c => c.IdentificationNumber == "00000000000")
                : _context.Clients.Where(c => c.IdentificationNumber != "00000000000");

            ViewData["ClientId"] = new SelectList(
                await clientes.ToListAsync(),
                "Id",
                "Name",
                Transaction.ClientId
            );

            // Documento: Ingresos x Ventas
            var documentType = await _context.DocumentTypes.FindAsync(Transaction.DocumentTypeId);
            var isIngresosXVentas = documentType?.Description == "Ingresos x Ventas";

            var documentos = isIngresosXVentas
                ? _context.DocumentTypes.Where(dt => dt.Description == "Ingresos x Ventas")
                : _context.DocumentTypes.Where(dt => dt.Description != "Ingresos x Ventas");

            ViewData["DocumentTypeId"] = new SelectList(
                await documentos.ToListAsync(),
                "Id",
                "Description",
                Transaction.DocumentTypeId
            );

            // Tipo de movimiento
            ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                .Cast<MovementType>()
                .Select(e => new { Id = e, Name = e.GetDisplayName() }),
                "Id", "Name", Transaction.MovementType);
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["Title"] = "Editar Transacción";

            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Client)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            Transaction = transaction;

            await CargarViewDataAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarViewDataAsync();
                return Page();
            }

            var client = await _context.Clients.FindAsync(Transaction.ClientId);

            if (client == null)
            {
                ModelState.AddModelError("", "El cliente seleccionado no existe.");
                await CargarViewDataAsync();

                return Page();
            }

            if (client.Status == Status.Inactive)
            {
                ModelState.AddModelError("Transaction.ClientId", "No se pueden realizar transacciones para clientes inactivos.");
                await CargarViewDataAsync();
                
                return Page();
            }

            if (Transaction.Amount > client.CreditLimit)
            {
                ModelState.AddModelError("Transaction.Amount", "El monto de la transacción no puede ser mayor al límite de crédito del cliente.");
                await CargarViewDataAsync();

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
