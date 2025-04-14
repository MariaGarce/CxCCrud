using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CRUDCxC.Data;
using CRUDCxC.Entities;

namespace CRUDCxC.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public IndexModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        public IList<Client> Client { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["Title"] = "Clientes";

            Client = await _context.Clients.ToListAsync();
        }
    }
}
