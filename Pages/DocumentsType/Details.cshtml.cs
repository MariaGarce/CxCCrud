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
    public class DetailsModel : PageModel
    {
        private readonly CRUDCxC.Data.CxCDbContext _context;

        public DetailsModel(CRUDCxC.Data.CxCDbContext context)
        {
            _context = context;
        }

        public DocumentType DocumentType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["Title"] = "Ver Tipo de Documento";

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
    }
}
