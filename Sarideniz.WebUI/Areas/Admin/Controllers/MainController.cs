using Microsoft.AspNetCore.Mvc;

namespace Sarideniz.WebUI.Areas.Admin.Controllers;

public class MainController : Controller
{
    [Area("Admin")]
    // GET
    public IActionResult Index()
    {
        return View();
    }
}