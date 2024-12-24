using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sarideniz.WebUI.Models;
using Sarideniz.Core.Entities;
using Sarideniz.Service.Abstract;

namespace Sarideniz.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IService<Product> _serviceProduct;
    private readonly IService<Slider> _serviceSlider;
    private readonly IService<News> _serviceNews;
    private readonly IService<Contact> _serviceContact;
    

    public HomeController(IService<Product> serviceProduct, IService<Slider>? serviceSlider,IService<News>? serviceNews,IService<Contact>? serviceContact)
    {
        _serviceProduct = serviceProduct;
        _serviceSlider =  serviceSlider;
        _serviceNews = serviceNews;
    }

    public async Task<IActionResult> Index()

    {
        var model = new HomePageViewModel()
        {
            Sliders = await _serviceSlider.GetAllAsync(),
            News = await _serviceNews.GetAllAsync(),
            Products = await _serviceProduct.GetAllAsync(p=>p.IsActive && p.IsHome),
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    [Route("AccessDenied")]
    public IActionResult AccessDenied()
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
          await _serviceContact.AddAsync(contact);
           var sonuc = await _serviceContact.SaveChangesAsync();
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