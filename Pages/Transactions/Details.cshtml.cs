using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CRUDCxC.Data;
using CRUDCxC.Entities;

namespace CRUDCxC.Pages.Transactions
{
    public class DetailsModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public DetailsModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        public Transaction Transaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["Title"] = "Ver Transacción";

            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Client)
                .Include(t => t.DocumentType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }
            else
            {
                Transaction = transaction;
            }
            return Page();
        }
    }
}
