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

namespace CRUDCxC.Pages.Transactions
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "IdentificationNumber");
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "AccountingAccount");
            ViewData["MovementTypes"] = new SelectList(Enum.GetValues(typeof(MovementType))
        .Cast<MovementType>()
        .Select(e => new { Id = e, Name = e.GetDisplayName() }), "Id", "Name");

            return Page();
        }

        [BindProperty]
        public Transaction Transaction { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Transactions.Add(Transaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
