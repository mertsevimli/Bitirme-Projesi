using System.ComponentModel.DataAnnotations;
using Sarideniz.Core.Entities;

public class Order : IEntity
{
    public int Id { get; set; }

    [Display(Name = "Sipariş No"), StringLength(50)]
    public string OrderNumber { get; set; }

    [Display(Name = "Sipariş Toplamı")]
    public decimal TotalPrice { get; set; }

    [Display(Name = "Müşteri No:")]
    public int AppUserId { get; set; }

    [Display(Name = "Müşteri"), StringLength(50)]
    public string CustomerId { get; set; } = Guid.NewGuid().ToString();

    [Display(Name = "Fatura Adresi"), StringLength(250)]
    public string BillingAddress { get; set; }

    [Display(Name = "Teslimat Adresi"), StringLength(250)]
    public string DeliveryAddress { get; set; }

    [Display(Name = "Sipariş Tarihi")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    [Display(Name = "Müşteri ")]
    public AppUser? AppUser{ get; set; }
    [Display(Name = "Sipariş Durumu ")]
    public EnumOrderState OrderState { get; set; }
    
}

public enum EnumOrderState
{
    [Display(Name = "Onay Bekleniyor ")]
    Waiting,
    [Display(Name = "Onaylandı ")]
    Approved,
    [Display(Name = "Kargoya verildi ")]
    Shipped,
    [Display(Name = "Tamamlandı ")]
    Completed,
    [Display(Name = "İptal Edildi ")]
    Cancelled,
    [Display(Name = "İade Edildi ")]
    Returned,
}