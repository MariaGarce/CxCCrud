using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CRUDCxC.Data;
using CRUDCxC.Entities;

namespace CRUDCxC.Pages.DocumentsType
{
    public class DeleteModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public DeleteModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DocumentType DocumentType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documenttype = await _context.DocumentTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (documenttype == null)
            {
                return NotFound();
            }
            else
            {
                DocumentType = documenttype;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documenttype = await _context.DocumentTypes.FindAsync(id);
            if (documenttype != null)
            {
                DocumentType = documenttype;
                _context.DocumentTypes.Remove(DocumentType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
