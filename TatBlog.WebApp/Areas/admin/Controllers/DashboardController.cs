using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.admin.Controllers;

public class DashboardController: Controller 
{
	public IActionResult Index()
	{
		return View();
	}
}
