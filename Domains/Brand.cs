using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAspEShop2024.Domains
{
    [Table("Brand")]
    public class Brand : BaseEntity
    {
        public override long Id => BrandId;

        [Key]
        public long BrandId { get; set; }

        public virtual List<Product> ProductsOfBrand { get; set; }
    }
}
