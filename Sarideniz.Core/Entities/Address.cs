using System.ComponentModel.DataAnnotations;
using System;

namespace Sarideniz.Core.Entities
{
    public class Address : IEntity
    {
        public int Id { get; set; }

        // Adres Başlığı
        [Display(Name = "Adres Başlığı")]
        [StringLength(50, ErrorMessage = "{0} Alanı en fazla {1} karakter olabilir.")]
        [Required(ErrorMessage = "{0} Alanı Zorunludur!.")]
        public string Title { get; set; }

        // Şehir
        [Display(Name = "Şehir")]
        [StringLength(100, ErrorMessage = "{0} Alanı en fazla {1} karakter olabilir.")]
        [Required(ErrorMessage = "{0} Alanı Zorunludur!.")]
        public string City { get; set; }

        // İlçe
        [Display(Name = "İlçe")]
        [StringLength(100, ErrorMessage = "{0} Alanı en fazla {1} karakter olabilir.")]
        [Required(ErrorMessage = "{0} Alanı Zorunludur!.")]
        public string District { get; set; }

        // Açık Adres
        [Display(Name = "Açık Adres")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "{0} Alanı Zorunludur!.")]
        public string OpenAddress { get; set; }

        // Aktiflik Durumu
        [Display(Name = "Aktiflik Durumu")]
        public bool IsActive { get; set; }

        // Fatura Adresi
        [Display(Name = "Fatura Adresi")]
        public bool IsBillingAddress { get; set; }

        // Teslimat Adresi
        [Display(Name = "Teslimat Adresi")]
        public bool IsDeliveryAddress { get; set; }

        // Kayıt Tarihi
        [Display(Name = "Kayıt Tarihi")]
        [ScaffoldColumn(false)]  // Bu alanın formda görünmesini engeller.
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // GUID ile Adres Tanımlaması
        [ScaffoldColumn(false)]  // Bu alanın formda görünmesini engeller.
        public Guid AddressGuid { get; set; } = Guid.NewGuid();

        // Kullanıcıya Bağlılık
        public int? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}