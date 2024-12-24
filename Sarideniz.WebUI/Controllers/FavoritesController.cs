using Microsoft.AspNetCore.Mvc;
using Sarideniz.Core.Entities;
using Sarideniz.Service.Abstract;
using Sarideniz.WebUI.ExtensionMethods;

namespace Sarideniz.WebUI.Controllers;

public class FavoritesController : Controller
{
    private readonly IService<Product> _service;

    public FavoritesController(IService<Product> service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var favoriler = GetFavorites();
        return View(favoriler);
    }

    private List<Product> GetFavorites()
    {
        return HttpContext.Session.GetJson<List<Product>>("GetFavorites") ?? [];
    }

    public IActionResult Add(int ProductId)
    {
        var favoriler = GetFavorites();
        var product = _service.Find(ProductId);

        if (product == null)
        {
            TempData["Error"] = "Ürün bulunamadı.";
            return RedirectToAction("Index");
        }

        if (!favoriler.Any(p => p.Id == product.Id))
        {
            favoriler.Add(product);
            HttpContext.Session.SetJson("GetFavorites", favoriler);
        }

        return RedirectToAction("Index");
    }

    public IActionResult Remove(int ProductId)
    {
        var favoriler = GetFavorites();
        var product = _service.Find(ProductId);

        if (favoriler.Any(p => p.Id == ProductId))
        {
            favoriler.RemoveAll(i => i.Id == ProductId);
            HttpContext.Session.SetJson("GetFavorites", favoriler);
        }

        return RedirectToAction("Index");
    }
}