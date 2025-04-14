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

namespace CRUDCxC.Pages.AccountEntries
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
            ViewData["Title"] = "Crear Asiento Contable";

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber");
            ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                .Cast<MovementType>()
                .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status))
                .Cast<Status>()
                .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");

            return Page();
        }

        [BindProperty]
        public AccountEntry AccountEntry { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber");
                ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                    .Cast<MovementType>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");
                ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status))
                    .Cast<Status>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");
                return Page();
            }

            var client = await _context.Clients.FindAsync(AccountEntry.ClientId);

            if (client == null)
            {
                ModelState.AddModelError("AccountEntry.ClientId", "El cliente seleccionado no existe.");
                ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber");
                ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                    .Cast<MovementType>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");
                ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status))
                    .Cast<Status>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");
                return Page();
            }

            if (client.Status == Status.Inactive)
            {
                ModelState.AddModelError("AccountEntry.ClientId", "No se puede realizar el siento contable porque el cliente está en estado 'Nuevo' o 'Inactivo'.");
                ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber");
                ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
                    .Cast<MovementType>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");
                ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status))
                    .Cast<Status>()
                    .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");
                return Page();
            }

            _context.AccountEntries.Add(AccountEntry);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

