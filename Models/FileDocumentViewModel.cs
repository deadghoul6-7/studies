using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Models
{
	public class FileDocumentViewModel
	{
		[Required]
		[Display(Name="Файл документа")]
        public IFormFile FormFile { get; set; }

		[Required]
		[Display(Name = "Название")]
        public string Name { get; set; }
    }
}
