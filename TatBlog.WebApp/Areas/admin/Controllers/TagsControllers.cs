using Microsoft.AspNetCore.Mvc;
namespace TatBlog.WebApp.Areas.admin.Controllers
{
	public class TagsControllers : Controller
	{
		public IAsyncResult Index()
		{
			return (IAsyncResult)View();
		}
	}
}
