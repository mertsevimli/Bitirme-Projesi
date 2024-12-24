//using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Sarideniz.Core.Entities;
using Sarideniz.Service.Abstract;

namespace Sarideniz.WebUI.ViewComponents;

public class Categories : ViewComponent
{
    private readonly IService<Category> _service;

    public Categories(IService<Category> service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _service.GetAllAsync(c =>c.IsTopMenu &&c.IsActive));
    }
}