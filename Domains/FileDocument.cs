using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAspEShop2024.Domains
{
	[Table("FileDocument")]
	public class FileDocument : BaseEntity
	{
		public override long Id => FileDocumentId;

		[Key]
		public long FileDocumentId { get; set; }

		public string StorageId { get; set; }
	}
}
