using ProjectAspEShop2024.AppIdentity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ProjectAspEShop2024.Domains
{
    public abstract class BaseEntity
    {
        [NotMapped]
        public virtual long Id { get; set; }

        public string Name { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual AppUser? ModifiedUser { get; set; }
    }
}
