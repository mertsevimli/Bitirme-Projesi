using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Sarideniz.Data;
using Sarideniz.WebUI.Models;
using Sarideniz.WebUI.Utils;
namespace Sarideniz.WebUI.Controllers;

public class ProductsController : Controller
{
    private readonly DatabaseContext _context;

    public ProductsController(DatabaseContext context)
    {
        _context = context;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var databaseContext = _context.Products.Where(p => p.IsActive).Include(p => p.Brand).Include(p => p.Category);
        return View(await  databaseContext.ToListAsync());
    }    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        var model = new ProductDetailsViewModel()
        {
            Product = product,
            RelatedProducts = _context.Products
                .Where(p => p.IsActive && p.CategoryId == product.CategoryId && p.Id != product.Id)
                .ToList() // Verileri listeye dönüştürmek için
        };
        return View(model);
        }
}