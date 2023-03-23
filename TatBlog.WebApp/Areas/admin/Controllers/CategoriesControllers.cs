using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.admin.Controllers

{
	public class CategoriesControllers : Controller
	{
		public IAsyncResult Index()
		{
			return (IAsyncResult)View();
		}
	}
}
