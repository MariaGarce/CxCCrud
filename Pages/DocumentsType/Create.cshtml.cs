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

namespace CRUDCxC.Pages.DocumentsType
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
            ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status))
.Cast<Status>()
.Select(e => new { Id = e, Name = e.GetDisplayName() }),
"Id", "Name");
            return Page();
        }

        [BindProperty]
        public DocumentType DocumentType { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["StatusList"] = new SelectList(Enum.GetValues(typeof(Status))
.Cast<Status>()
.Select(e => new { Id = e, Name = e.GetDisplayName() }),
"Id", "Name");
                return Page();
                return Page();
            }

            _context.DocumentTypes.Add(DocumentType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
