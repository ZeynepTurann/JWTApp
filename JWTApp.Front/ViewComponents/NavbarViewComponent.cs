﻿using Microsoft.AspNetCore.Mvc;

namespace JWTApp.Front.ViewComponents
{
	public class NavbarViewComponent:ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
