using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarideniz.Core.Entities;
using Sarideniz.Service.Abstract;

namespace Sarideniz.WebUI.Controllers;

[Authorize]
public class MyAddressesController : Controller
{
    private readonly IService<AppUser> _serviceAppUser;
    private readonly IService<Address> _serviceAddress;

    public MyAddressesController(IService<AppUser> service, IService<Address> serviceAddress)
    {
        _serviceAppUser = service;
        _serviceAddress = serviceAddress;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var appUser =
            await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
        if (appUser == null)
        {
            return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapınız!");
        }

        var model = await _serviceAddress.GetAllAsync(u => u.AppUserId == appUser.Id);
        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Address address)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var appUser = await _serviceAppUser.GetAsync(x =>
                    x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                if (appUser != null)
                {
                    address.AppUserId = appUser.Id;
                    _serviceAddress.Add(address);
                    await _serviceAddress.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
        }

        ModelState.AddModelError("", "Kayıt Başarısız!");
        return View(address);
    }
    
    public async Task<IActionResult> Edit(string id)
    {
        var appUser =
            await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
        if (appUser == null)
        {
            return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapınız!");
        }
        var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);
        if (model == null)
        {
            return NotFound("Adres bilgisi  Bulunamadı!");
        }
        return View(model);
   
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id,Address address)
    {
        var appUser =
            await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
        if (appUser == null)
        {
            return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapınız!");
        }
        var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);

        if (model == null)
        {
            return NotFound("Adres bilgisi bulunamadı!");
        }

// Eğer model null değilse işlemlere devam et
        model.Title = address.Title;
        model.District = address.District;
        model.City = address.City;
        model.OpenAddress = address.OpenAddress;
        model.IsDeliveryAddress = address.IsDeliveryAddress;
        model.IsBillingAddress = address.IsBillingAddress;
        model.IsActive = address.IsActive;

// Diğer adresleri al ve güncelle
        var otherAddresses = await _serviceAddress.GetAllAsync(x => x.AppUserId == appUser.Id && x.Id != model.Id);
        foreach (var otherAddress in otherAddresses)
        {
            otherAddress.IsDeliveryAddress = false;
            otherAddress.IsBillingAddress = false;
            _serviceAddress.Update(otherAddress);
        }

// Model ve diğer adresler üzerinde güncellemeleri kaydet
        try
        {
            _serviceAddress.Update(model);
            await _serviceAddress.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "Hata oluştu!");
        }
        return View(model);
   
    }
    public async Task<IActionResult> Delete(string id)
    {
        var appUser =
            await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
        if (appUser == null)
        {
            return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapınız!");
        }
        var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);
        if (model == null)
        {
            return NotFound("Adres bilgisi  Bulunamadı!");
        }
        return View(model);
   
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id, Address address)
    {
        var appUser =
            await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
        if (appUser == null)
        {
            return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapınız!");
        }
        var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);
        if (model == null)
        {
            return NotFound("Adres bilgisi  Bulunamadı!");
        }

        try
        {
            _serviceAddress.Delete(model);
            await _serviceAddress.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
           ModelState.AddModelError("", "Hata Oluştu");
        }
        return View(model);
   
    }
}