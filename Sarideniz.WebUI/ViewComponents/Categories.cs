using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Sarideniz.Data;

namespace Sarideniz.WebUI.ViewComponents;

public class Categories : ViewComponent
{
    private readonly DatabaseContext _context;

    public Categories(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.Categories.ToListAsync());
    }
}