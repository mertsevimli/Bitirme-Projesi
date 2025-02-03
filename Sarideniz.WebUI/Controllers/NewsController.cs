using Microsoft.AspNetCore.Mvc;
using Sarideniz.Core.Entities;
using Sarideniz.Service.Abstract;

namespace Sarideniz.WebUI.Controllers;

public class NewsController : Controller
{
    private readonly IService<News> _service;

    public NewsController(IService<News> service)
    {
        _service = service;
    }
    public async Task<IActionResult> Index()
    {
        return View(await _service.GetAllAsync());
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound("Geçersiz İstek");
        }

        var news = await _service
            .GetAsync(m => m.Id == id && m.IsActive);
        if (news == null)
        {
            return NotFound("Gerçerli Bir Kampanya Bulunamadı!");
        }

        return View(news);
    }
}

