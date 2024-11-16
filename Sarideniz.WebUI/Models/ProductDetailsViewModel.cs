using Sarideniz.Core.Entities;

namespace Sarideniz.WebUI.Models;

public class ProductDetailsViewModel
{
    public Product Product { get; set; }
    public IEnumerable<Product>? RelatedProducts { get; set; }
}