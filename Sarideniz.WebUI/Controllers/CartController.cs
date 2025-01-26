using Microsoft.AspNetCore.Authorization;
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
    private readonly IService<Address> _serviceAddress;
    private readonly IService<AppUser> _serviceAppUser;

    public CartController (IService<Product> serviceProduct, IService<Address> serviceAddress, IService<AppUser> serviceAppUser)
    {
        _serviceProduct = serviceProduct;
        _serviceAddress = serviceAddress;
        _serviceAppUser = serviceAppUser;
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
    [Authorize]
    public async Task<IActionResult> Checkout()
    {
        var cart = GetCart();
        var appUser = await _serviceAppUser.GetAsync(x=>x.UserGuid.ToString()== HttpContext.User.FindFirst("UserGuid").Value);
        if (appUser == null)
        {
            return RedirectToAction("SignIn", "Account");
        }
        var addresses = await _serviceAddress.GetAllAsync(a=>a.AppUserId == appUser.Id && a.IsActive);
        var model = new CheckoutViewModel()
        {
            CartProducts = cart.CartLines,
            TotalPrice = cart.TotalPrice(),
            Addresses = addresses
        };
            
        return View(model);
    }
    [Authorize, HttpPost]
    public async Task<IActionResult> Checkout(string CardNumber, string CardMonth, string CardYear, string CVV, string Addresses, string BillingAddress)
    {
        var cart = GetCart();
        var appUser = await _serviceAppUser.GetAsync(x=>x.UserGuid.ToString()== HttpContext.User.FindFirst("UserGuid").Value);
        if (appUser == null)
        {
            return RedirectToAction("SignIn", "Account");
        }
        var addresses = await _serviceAddress.GetAllAsync(a=>a.AppUserId == appUser.Id && a.IsActive);
        var model = new CheckoutViewModel()
        {
            CartProducts = cart.CartLines,
            TotalPrice = cart.TotalPrice(),
            Addresses = addresses
        };
        if (string.IsNullOrWhiteSpace(CardNumber) ||string.IsNullOrWhiteSpace(CardMonth)|| string.IsNullOrWhiteSpace(CardYear)|| string.IsNullOrWhiteSpace(CVV)|| string.IsNullOrWhiteSpace(Addresses)|| string.IsNullOrWhiteSpace(BillingAddress))
        {
            return View(model);
        }

        var teslimatAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == Addresses); 
        var faturaAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == BillingAddress);  
        return View(model);
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