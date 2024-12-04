using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sarideniz.WebUI.Areas.Admin.Controllers;

public class MainController : Controller
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    // GET
    public IActionResult Index()
    {
        return View();
    }
}