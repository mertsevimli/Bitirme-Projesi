using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Sarideniz.Service.Abstract;

namespace Sarideniz.WebUI.Controllers;

public class CategoriesController : Controller
{
    private readonly IService<Category> _service;

    public CategoriesController(IService<Category> service)
    {
        _service = service;
    }
    // GET
    public async Task<IActionResult> Index(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _service.GetQueryable().Include(p=>p.Products)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
}