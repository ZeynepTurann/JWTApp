namespace JWTApp.Front.Models
{
	public class ProductListModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
		public CategoryListModel Category { get; set; } = new CategoryListModel();
	}
}
