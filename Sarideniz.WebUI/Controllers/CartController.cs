using Microsoft.AspNetCore.Mvc;
using Sarideniz.Core.Entities;
using Sarideniz.Service.Abstract;
using Sarideniz.Service.Concrete;
using Sarideniz.WebUI.ExtensionMethods;
using Sarideniz.WebUI.Models;

namespace Sarideniz.WebUI.Controllers;

public class CartController : Controller
{
    private readonly IService<Product> _serviceProduct;

    public CartController(IService<Product> serviceProduct)
    {
        _serviceProduct = serviceProduct;
    }

    // GET
    public IActionResult Index()
    {
        var cart = GetCart();
        var model = new CartViewModel()
        {
            CartLines = cart.CartLines,
            TotalPrice = cart.TotalPrice()
        };
            
        return View(model);
    }

    private CartService GetCart()
    {
        return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
    }

    public IActionResult Add(int productId, int quantity = 1)
    {
        var product = _serviceProduct.Find(productId);
        if (product != null)
        {
            var cart = GetCart();
            cart.AddProduct(product, quantity);
            HttpContext.Session.SetJson("Cart", cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        return RedirectToAction("Index");
    }
    public IActionResult Update(int productId, int quantity = 1)
    {
        var product = _serviceProduct.Find(productId);
        if (product != null)
        {
            var cart = GetCart();
            cart.UpdateProduct(product, quantity);
            HttpContext.Session.SetJson("Cart", cart);
        }

        return RedirectToAction("Index");
    }
    public IActionResult Remove(int ProductId)
    {
        var product = _serviceProduct.Find(ProductId);
        if (product != null)
        {
            var cart = GetCart();
            cart.RemoveProduct(product);
            HttpContext.Session.SetJson("Cart", cart);
        }

        return RedirectToAction("Index");
    }
}