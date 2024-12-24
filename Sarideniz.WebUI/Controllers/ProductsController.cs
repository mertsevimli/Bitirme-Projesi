using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Sarideniz.Data;
using Sarideniz.Service.Abstract;
using Sarideniz.WebUI.Models;
using Sarideniz.WebUI.Utils;
namespace Sarideniz.WebUI.Controllers;

public class ProductsController : Controller
{

    private readonly IService<Product> _serviceProduct;

    public ProductsController(IService<Product> serviceProduct)
    {
        _serviceProduct = serviceProduct;
    }


    // GET
    public async Task<IActionResult> Index(string arama = "")
    {
        var databaseContext =
            _serviceProduct.GetAllAsync(p => p.IsActive && p.Name.Contains(arama) || p.Description.Contains(arama));
        return View(await  databaseContext);
    }    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _serviceProduct.GetQueryable()
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
            RelatedProducts = _serviceProduct.GetQueryable()
                .Where(p => p.IsActive && p.CategoryId == product.CategoryId && p.Id != product.Id)
                .ToList() // Verileri listeye dönüştürmek için
        };
        return View(model);
        }
}