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
    public class IndexModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public IndexModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        public IList<AccountEntry> AccountEntry { get;set; } = default!;

        public async Task OnGetAsync()
        {
            AccountEntry = await _context.AccountEntries
                .Include(a => a.Client).ToListAsync();
        }
    }
}
