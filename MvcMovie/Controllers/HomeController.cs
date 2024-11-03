using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MvcMovie.Models;
using System.Diagnostics;

namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly IStringLocalizer<HomeController> _localizer;
		public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
		{
			_logger = logger;
			_localizer = localizer;
		}

		public IActionResult Index()
		{
			if (_localizer["welcome"] != null)
				ViewBag.welcome = string.Format(_localizer["welcome"], "Programmer");
			else
				ViewBag.welcome = "not any data in here";

			return View();
		}
		// Knowing the culture that is currently used and the quality in which it is used
		[HttpPost]
		public IActionResult SetLanguage(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
				);

			return LocalRedirect(returnUrl);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
