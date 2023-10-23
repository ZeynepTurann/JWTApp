using System.ComponentModel.DataAnnotations;

namespace JWTApp.Front.Models
{
	public class CategoryCreateModel
	{
		[Required(ErrorMessage ="Definition is required!")]
		public string? Definition { get; set; }
	}
}
