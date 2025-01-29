using System.ComponentModel.DataAnnotations;
using Sarideniz.Core.Entities;

public class OrderLine : IEntity
{
    public int Id { get; set; }

    [Display(Name = "Sipariş No")]
    public int OrderId { get; set; }

    [Display(Name = "Sipariş")]
    [Required(ErrorMessage = "Sipariş zorunludur.")]
    public Order Order { get; set; }  // Order zorunlu

    [Display(Name = "Ürün No")]
    public int ProductId { get; set; }

    [Display(Name = "Ürün")]
    public Product Product { get; set; }

    [Display(Name = "Miktar")]
    public int Quantity { get; set; }

    [Display(Name = "Birim Fiyat")]
    public decimal UnitPrice { get; set; }
}