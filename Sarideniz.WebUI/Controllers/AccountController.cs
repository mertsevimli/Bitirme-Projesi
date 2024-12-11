using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Sarideniz.Data;
using Sarideniz.WebUI.Models;


namespace Sarideniz.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly DatabaseContext _context;

    public AccountController(DatabaseContext context)
    {
        _context = context;
    }
    [Authorize]
    // GET
    public IActionResult Index()
    {
        AppUser user = _context.AppUsers.FirstOrDefault(x=>x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
        if (user is null)
        {
            return NotFound();
        }var model = new UserEditViewModel()
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
    public IActionResult Index(UserEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                
                AppUser user = _context.AppUsers.FirstOrDefault(x=>x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                if (user is not null)
                {
                  user.Name = model.Name;
                  user.Surname = model.Surname;
                  user.Email = model.Email;
                  user.Phone = model.Phone;
                  user.Password = model.Password;
                  user.Phone = model.Phone;
                  _context.AppUsers.Update(user);
                 var sonuc = _context.SaveChanges();
                  
                  if (sonuc > 0)
                  {
                      TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
  <strong>Hesap Bilgileriniz Güncellenmiştir.!</strong>
  <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
</div>";
                      //await MailHelper.SendEmailAsync(contact);
                      return RedirectToAction("Index");
                  }
                }

                
            }
            catch (Exception e)
            {
                
                ModelState.AddModelError("", "Hata Oluştu!");
            }  
        }
        return View(model);
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
            var account = await _context.AppUsers.FirstOrDefaultAsync(x => x.Email == loginViewModel.Email && x.Password == loginViewModel.Password && x.IsActive);
            if (account == null)
            {
                ModelState.AddModelError("","Hata Oluştu!");
            }
            else
            {
                var claims = new List<Claim>()
                {
                    new (ClaimTypes.Name, account.Name),
                    new (ClaimTypes.Role, account.IsAdmin ? "Admin" : "Customer"),
                    new (ClaimTypes.Email, account.Email),
                    new ("UserId", account.Id.ToString()),
                    new ("UserGuid", account.UserGuid.ToString()),
                };
                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(userPrincipal);
                return Redirect(string.IsNullOrEmpty(loginViewModel.ReturnUrl) ? "/" : loginViewModel.ReturnUrl);
            }
            }
            catch (Exception hata)
            {
               ModelState.AddModelError("","Hata Oluştu!");
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
            await _context.AddAsync(appUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(appUser);
    }   public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("SignIn");
    }
}