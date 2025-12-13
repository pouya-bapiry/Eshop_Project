using Eshop.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.Product
{
    public class ProductDetailsDto
    {
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int? View { get; set; }
        public List<Entities.Product.ProductGallery> ProductGalleries { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<Entities.Product.ProductCategory> ProductCategories { get; set; }
        public List<Entities.Product.ProductFeatures> ProductFeatures { get; set; }
        public List<Entities.Product.Product> RelatedProducts { get; set; }
        //public List<Entities.ProductComment.ProductComment> ProductComments { get; set; }
        public Entities.ProductDiscount.ProductDiscount ProductDiscount { get; set; }
        //public ProductBrand ProductBrand { get; set; }
    }
}
