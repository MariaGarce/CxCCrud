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

namespace CRUDCxC.Pages.DocumentsType
{
    public class EditModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public EditModel(CRUDCxC.Data.CxCDbContext context)
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
            DocumentType = documenttype;

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

            _context.Attach(DocumentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentTypeExists(DocumentType.Id))
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

        private bool DocumentTypeExists(int id)
        {
            return _context.DocumentTypes.Any(e => e.Id == id);
        }
    }
}
