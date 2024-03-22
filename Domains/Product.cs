using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAspEShop2024.Domains
{
    [Table("Product")]
    public class Product : BaseEntity
    {
        public override long Id => ProductId;

        [Key]
        public long ProductId { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public string? PhotoUrl { get; set; }

        public string? Description { get; set; }

        public virtual Category? CategoryOfProduct { get; set; }

        public virtual Brand? BrandOfProduct { get; set; }
    }
}
