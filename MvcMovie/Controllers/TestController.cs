﻿using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers
{
	public class TestController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Create()
		{
			return View();
		}
	}
}
