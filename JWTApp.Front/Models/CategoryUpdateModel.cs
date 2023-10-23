using System.ComponentModel.DataAnnotations;

namespace JWTApp.Front.Models
{
	public class CategoryUpdateModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Definition is required!")]
		public string? Definition { get; set; }
	}
}
