using JWTApp.Front.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace JWTApp.Front.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
	
		public CategoryController(IHttpClientFactory httpClientFactory)
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
				var response = await client.GetAsync("http://localhost:5253/api/categories");
				if (response.IsSuccessStatusCode)
				{
					var jsonData = await response.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<List<CategoryListModel>>(jsonData, new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});
					return View(result);
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
				var response = await client.DeleteAsync($"http://localhost:5253/api/categories/{id}");
			}
			return RedirectToAction("List");
		}

		public IActionResult Create()
		{
			return View(new CategoryCreateModel());
		}

		[HttpPost]
		public async Task<IActionResult> Create(CategoryCreateModel model)
		{
			if (ModelState.IsValid)
			{
				var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

				if (token != null)
				{
					var client = this.CreateClient(token);
					var jsonData = JsonSerializer.Serialize(model);
					var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
					var response = await client.PostAsync($"http://localhost:5253/api/categories", content);
					if (response.IsSuccessStatusCode)
					{
						return RedirectToAction("List");
					}
					else
					{
						ModelState.AddModelError("", "An error has occurred!");
					}
				}
			}
			return View(model);
		}

		public async Task<IActionResult> Update(int id)
		{
			var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;

			if (token != null)
			{
				var client = this.CreateClient(token);
				var response = await client.GetAsync($"http://localhost:5253/api/categories/{id}");
				if (response.IsSuccessStatusCode)
				{
					var jsonData = await response.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<CategoryUpdateModel>(jsonData, new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});
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
		public async Task<IActionResult> Update(CategoryUpdateModel model)
		{
			if (ModelState.IsValid)
			{
				var token = User.Claims.FirstOrDefault(x => x.Type == "accessToken")?.Value;
				if (token != null)
				{
					var client = this.CreateClient(token);
					var jsonData = JsonSerializer.Serialize(model);
					var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
					var response = await client.PutAsync($"http://localhost:5253/api/categories", content);
					if (response.IsSuccessStatusCode)
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
			return View(model);
		}
	}
}
