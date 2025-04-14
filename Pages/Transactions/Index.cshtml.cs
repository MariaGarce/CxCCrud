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
    public class IndexModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public IndexModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        public IList<Transaction> Transaction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["Title"] = "Transacciones";

            Transaction = await _context.Transactions
                .Include(t => t.Client)
                .Include(t => t.DocumentType).ToListAsync();
        }
    }
}
