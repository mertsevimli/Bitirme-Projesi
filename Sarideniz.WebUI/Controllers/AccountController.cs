using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Sarideniz.Service.Abstract;
using Sarideniz.WebUI.Models;
using Sarideniz.WebUI.Utils;

namespace Sarideniz.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly IService<AppUser> _service;
    private readonly IService<Order> _serviceOrder;

    public AccountController(IService<AppUser> service, IService<Order> serviceOrder)
    {
        _service = service;
        _serviceOrder = serviceOrder;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        AppUser user = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserEditViewModel()
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Password = user.Password,
            Phone = user.Phone,
        };

        return View(model);
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Index(UserEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Email = model.Email;
                    user.Phone = model.Phone;
                    user.Password = model.Password;

                    _service.Update(user);
                    var result = await _service.SaveChangesAsync();
                    
                    if (result > 0)
                    {
                        TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
  <strong>Hesap Bilgileriniz Güncellenmiştir.!</strong>
  <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
</div>";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Message"] = "Kullanıcı bulunamadı.";
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Bir hata oluştu! Lütfen tekrar deneyin.");
            }
        }

        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
        if (user == null)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        var orders = await _serviceOrder.GetQueryable()
            .Where(s => s.AppUserId == user.Id)
            .Include(o => o.OrderLines)
                .ThenInclude(p => p.Product)
            .ToListAsync();

        return View(orders);
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignInAsync(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var account = await _service.GetAsync(x => x.Email == loginViewModel.Email && x.Password == loginViewModel.Password && x.IsActive);
                if (account == null)
                {
                    ModelState.AddModelError("", "Geçersiz giriş bilgileri!");
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.Name),
                        new Claim(ClaimTypes.Role, account.IsAdmin ? "Admin" : "Customer"),
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim("UserId", account.Id.ToString()),
                        new Claim("UserGuid", account.UserGuid.ToString())
                    };

                    var userIdentity = new ClaimsIdentity(claims, "Login");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(userPrincipal);

                    return Redirect(string.IsNullOrEmpty(loginViewModel.ReturnUrl) ? "/" : loginViewModel.ReturnUrl);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Bir hata oluştu! Lütfen tekrar deneyin.");
            }
        }
        return View(loginViewModel);
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(AppUser appUser)
    {
        appUser.IsAdmin = false;
        appUser.IsActive = true;

        if (ModelState.IsValid)
        {
            await _service.AddAsync(appUser);
            await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(appUser);
    }

    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("SignIn");
    }
    public IActionResult PasswordRenew()
    {
        
        return View();
    }
   
    [HttpPost]
    public async Task<IActionResult> PasswordRenew(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ModelState.AddModelError("","Email Boş Geçilemez!");
            return View();
        }
        AppUser user = await _service.GetAsync(x => x.Email == Email);
        if (user == null)
        {
            ModelState.AddModelError("","Giriş yaptığınız Email adresi bulunamadı!");
            return View();
        }

        string mesaj = $"Sayın {user.Name}{user.Surname} <hr> Şifrenizi Yenilemek için Lütfen <a href='http://localhost:5257/Account/PasswordChange?user={user.UserGuid.ToString()}'>Buraya Tıklayınız</a> ";
        var sonuc = await MailHelper.SendEmailAsync(Email,"Şifremi Yenile",mesaj);
        if (sonuc)
        {
            TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
  <strong>Şifre Sıfırlama Bağlantınız Mail Adresinize Gönderilmiştir..</strong>
  <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
</div>";
        }
        else
        {
            TempData["Message"] = @"<div class=""alert alert-danger alert-dismissible fade show"" role=""alert"">
  <strong>Şifre Sıfırlama Bağlantınız Mail Adresinize Gönderilmedi!.</strong>
  <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
</div>";
        }
        //
        return View();
    }
    public async Task<IActionResult> PasswordChange(string user)
    {
        if (user is null)
        {
            return BadRequest("Geçersiz İstek!");
        }
        AppUser appUser = await _service.GetAsync(x => x.UserGuid.ToString() == user);
        if (appUser == null)
        {
            ModelState.AddModelError("","Geçersiz Değer");
            return NotFound("Geçersiz Değer!");
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> PasswordChange(string user, string Password)
    {
        if (user is null)
        {
            return BadRequest("Geçersiz İstek!");
        }
        AppUser appUser = await _service.GetAsync(x => x.UserGuid.ToString() == user);
        if (appUser == null)
        {
            ModelState.AddModelError("","Geçersiz Değer");
            return View();
        }

        appUser.Password = Password;
         var sonuc = await _service.SaveChangesAsync();
         if (sonuc > 0)
         {
             TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
  <strong>Şifreniz Güncellenmiştir! Giriş Ekranından Oturum Açabilirsiniz.</strong>
  <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
</div>";
         }
         else
         {
             ModelState.AddModelError("","Güncelleme Başarısız!");
         }
        return View();
    }
}