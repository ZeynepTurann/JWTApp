using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JWTApp.Front.Models
{
	public class ProductUpdateModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Definition is required!")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Stock is required!")]
		public int Stock { get; set; }
		[Required(ErrorMessage = "Price is required!")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Category is required!")]
		public int CategoryId { get; set; }
		public SelectList? Categories { get; set; }
	}
}
