using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CRUDCxC.Data;
using CRUDCxC.Entities;

namespace CRUDCxC.Pages.AccountEntries
{
    public class DeleteModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public DeleteModel(CRUDCxC.Data.CxCDbContext context)
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

            var accountentry = await _context.AccountEntries
                .Include(a => a.Client) // Asegura que Client se cargue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountentry == null)
            {
                return NotFound();
            }
            else
            {
                AccountEntry = accountentry;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountentry = await _context.AccountEntries.FindAsync(id);
            if (accountentry != null)
            {
                AccountEntry = accountentry;
                _context.AccountEntries.Remove(AccountEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
