using Sarideniz.Core.Entities;

namespace Sarideniz.WebUI.Models;

public class CheckoutViewModel
{
    public List<CartLine>? CartProducts { get; set; }
    public decimal TotalPrice { get; set; }
}