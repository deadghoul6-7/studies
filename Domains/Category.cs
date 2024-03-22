using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAspEShop2024.Domains
{
    [Table("Category")]
    public class Category : BaseEntity
    {
        public override long Id => CategoryId;

        [Key]
        public long CategoryId { get; set; }

        public virtual List<Product> ProductsOfCategory { get; set; }
    }
}
