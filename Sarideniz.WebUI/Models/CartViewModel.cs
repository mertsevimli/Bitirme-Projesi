using Sarideniz.Core.Entities;

namespace Sarideniz.WebUI.Models;

public class CartViewModel
{
    public List<CartLine>? CartLines { get; set; }
    public decimal TotalPrice { get; set; }
    
}