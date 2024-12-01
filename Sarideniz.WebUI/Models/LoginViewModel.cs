using System.ComponentModel.DataAnnotations;

namespace Sarideniz.WebUI.Models;

public class LoginViewModel
{
    [DataType(DataType.EmailAddress), Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
    public string Email { get; set; }
    [Display(Name = "Şifre"), Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")] 
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }
    public bool RememberMe { get; set; }
   
    
}