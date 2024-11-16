using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarideniz.Data;

namespace Sarideniz.WebUI.Controllers;

public class CategoriesController : Controller
{
    private readonly DatabaseContext _context;

    public CategoriesController(DatabaseContext context)
    {
        _context = context;
    }
    // GET
    public async Task<IActionResult> Index(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.Include(p=>p.Products)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
}