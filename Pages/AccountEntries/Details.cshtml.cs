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
    public class DetailsModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public DetailsModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        public AccountEntry AccountEntry { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["Title"] = "Ver Asiento Contable";

            if (id == null)
            {
                return NotFound();
            }

            var accountentry = await _context.AccountEntries.FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
