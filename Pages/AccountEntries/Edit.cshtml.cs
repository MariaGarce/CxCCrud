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

namespace CRUDCxC.Pages.AccountEntries
{
    public class EditModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public EditModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccountEntry AccountEntry { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountentry = await _context.AccountEntries.FirstOrDefaultAsync(m => m.Id == id);
            if (accountentry == null)
            {
                return NotFound();
            }
            AccountEntry = accountentry;
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber");
            ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
     .Cast<MovementType>()
     .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status))
           .Cast<Status>()
           .Select(e => new { Id = e, Name = e.GetDisplayName() }),
           "Id", "Name");


            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AccountEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountEntryExists(AccountEntry.Id))
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

        private bool AccountEntryExists(int id)
        {
            return _context.AccountEntries.Any(e => e.Id == id);
        }
    }
}
