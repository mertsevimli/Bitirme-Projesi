using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sarideniz.Data;
using Sarideniz.WebUI.Models;
using Microsoft.EntityFrameworkCore;

namespace Sarideniz.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly DatabaseContext _context;

    public HomeController(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Sliders.ToListAsync());
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult ContactUs()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}