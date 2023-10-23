using JWTApp.Front.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace JWTApp.Front.Controllers
{
	[Authorize(Roles = "Admin,Member")]
	public class ProductController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ProductController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		private HttpClient CreateClient(string token)
		{
			var client = _httpClientFactory.CreateClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
			return client;
		}

		public async Task<IActionResult> List()
		{
			var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

			if (token != null)
			{
				var client = this.CreateClient(token);
				var response = await client.GetAsync("http://localhost:5253/api/products");
				if (response.IsSuccessStatusCode)
				{
					var jsonData = await response.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<List<ProductListModel>>(jsonData, new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});

					if(result != null)
					{
						foreach(var product in result)
						{
							var responseCat = await client.GetAsync($"http://localhost:5253/api/categories/{product.CategoryId}");
							var categoryJsonData = await responseCat.Content.ReadAsStringAsync();
							var resultCat = JsonSerializer.Deserialize<CategoryListModel>(categoryJsonData, new JsonSerializerOptions
							{
								PropertyNamingPolicy = JsonNamingPolicy.CamelCase
							});
							if (resultCat != null)
								product.Category = resultCat;
						}
						return View(result);
					}
					
				}

			}
			return View();
		}

		public async Task<IActionResult> Remove(int id)
		{
			var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
			if (token != null)
			{
				var client = this.CreateClient(token);
				var response = await client.DeleteAsync($"http://localhost:5253/api/products/{id}");
			}
			return RedirectToAction("List");
		}

		public async Task<IActionResult> Create()
		{
			ProductCreateModel model = new();
			var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
			if (token != null)
			{
				var client = this.CreateClient(token);
				var response = await client.GetAsync("http://localhost:5253/api/categories");
				if (response.IsSuccessStatusCode)
				{
					var jsonData = await response.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<List<CategoryListModel>>(jsonData, new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});
					model.Categories = new SelectList(result, "Id", "Definition");
					return View(model);
				}
			}
			return RedirectToAction("List");
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProductCreateModel model)
		{
			if (ModelState.IsValid)
			{
				var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
				if (token != null)
				{
					var client = this.CreateClient(token);

					var jsonData = JsonSerializer.Serialize(model);
					var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
					var response = await client.PostAsync($"http://localhost:5253/api/products", content);
					if (response.IsSuccessStatusCode)
					{
						return RedirectToAction("List");
					}
					ModelState.AddModelError("", "An error has occurred!");
				}
			}
			//We moved categories from view to method via TempData
		   var data = TempData["Categories"]?.ToString();
			if (data != null)
			{
				var categories = JsonSerializer.Deserialize<List<SelectListItem>>(data);
				model.Categories = new SelectList(categories, "Value", "Text");
			}
			return View(model);
		}

		public async Task<IActionResult> Update(int id)
		{
			var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

			if (token != null)
			{
				var client = this.CreateClient(token);
				var responseProduct = await client.GetAsync($"http://localhost:5253/api/products/{id}");
				if (responseProduct.IsSuccessStatusCode)
				{
					var jsonData = await responseProduct.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<ProductUpdateModel>(jsonData, new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});

					//adding categories
					var responseCategory = await client.GetAsync("http://localhost:5253/api/categories");
					if (responseCategory.IsSuccessStatusCode)
					{
						var json = await responseCategory.Content.ReadAsStringAsync();
						var resultCategory = JsonSerializer.Deserialize<List<CategoryListModel>>(json, new JsonSerializerOptions
						{
							PropertyNamingPolicy = JsonNamingPolicy.CamelCase
						});
						if (result != null)
						{
							result.Categories = new SelectList(resultCategory, "Id", "Definition", result.CategoryId);
						}
					}

					return View(result);

				}
				else
				{
					ModelState.AddModelError("", "An error has occurred!");
				}

			}
			return RedirectToAction("List");
		}

		[HttpPost]
		public async Task<IActionResult> Update(ProductUpdateModel model)
		{
			if (ModelState.IsValid)
			{
				var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
				if (token != null)
				{
					var client = this.CreateClient(token);
					var jsonData = JsonSerializer.Serialize(model);
					var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
					var response = await client.PutAsync("http://localhost:5253/api/products", content);

					if(response.IsSuccessStatusCode)
					{
						return RedirectToAction("List");
					}
					else
					{
						ModelState.AddModelError("", "An error has occurred!");
					}
				}
				return RedirectToAction("List");

			}
			var data = TempData["Categories"]?.ToString(); 
			if (data != null)
			{
				var categories = JsonSerializer.Deserialize<List<SelectListItem>>(data);
				model.Categories = new SelectList(categories, "Value", "Text",model.CategoryId);
			}
			return View(model);
		}
	}

}
