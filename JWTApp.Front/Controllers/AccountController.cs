using JWTApp.Front.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace JWTApp.Front.Controllers
{
	public class AccountController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public AccountController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public IActionResult Login()
		{
			return View(new UserLoginModel());
		}

		[HttpPost]
		public async Task<IActionResult> Login(UserLoginModel model)
		{
			if (ModelState.IsValid)
			{
				var client = _httpClientFactory.CreateClient();
				var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
				var response = await client.PostAsync("http://localhost:5253/api/Auth/Login", content);
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var tokenModel = JsonSerializer.Deserialize<JwtTokenResponseModel>(json, new JsonSerializerOptions
					{
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase
					});

					if (tokenModel != null)
					{
						JwtSecurityTokenHandler handler = new();
						var token = handler.ReadJwtToken(tokenModel.Token); //tekrardan handler üzerinden string formatındaki toke'ı jwtsecuritytoken olarak elde ettik

						var claims = token.Claims.ToList();

						claims.Add(new Claim("accessToken", tokenModel.Token == null ? "" : tokenModel.Token));

						var claimsIndentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

						var authProps = new AuthenticationProperties
						{
							ExpiresUtc = tokenModel.ExpireDate,
							IsPersistent = true,   //Do not log in repeatedly until the token expires



						};
						await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIndentity), authProps);
						return RedirectToAction("Index", "Home");
					}
					else
					{
						ModelState.AddModelError("", "Username or password is wrong!");
						return View(model);
					}
				}
				else
				{
					ModelState.AddModelError("", "Username or password is wrong!");
					return View(model);
				}

				
			}
			return View(model);
		}
	}
}
