using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sarideniz.Data;
using Sarideniz.WebUI.Models;
using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Sarideniz.WebUI.Utils;

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
        var model = new HomePageViewModel()
        {
            Sliders = await _context.Sliders.ToListAsync(),
            News = await _context.News.ToListAsync(),
            Products = await _context.Products.Where(p=>p.IsActive && p.IsHome).ToListAsync(),
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult ContactUs()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ContactUs(Contact contact)
    {
        if (ModelState.IsValid)
        {
            try
            {
          await _context.Contacts.AddAsync(contact);
           var sonuc = await _context.SaveChangesAsync();
          if (sonuc > 0)
           {
               TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
  <strong>Mesajınız Gönderilmiştir.</strong>
  <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
</div>";
               //await MailHelper.SendEmailAsync(contact);
               return RedirectToAction("ContactUs");
           }
            }
            catch (Exception e)
            {
               ModelState.AddModelError("", "Hata Oluştu!");
            }
        }
        return View(contact);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}