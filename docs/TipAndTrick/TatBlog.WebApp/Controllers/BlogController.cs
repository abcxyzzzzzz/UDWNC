using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Models;

namespace TatBlog.WebApp.Controllers
{
	public class BlogController : Controller
	{
		public async Task<IActionResult> Index()
		{
			[FromQuery(Name = "p")] int pageNumber = 1;
			[FromQuery(Name = "ps")] int pageSize = 10;
			{
				var postQuery = new PostQuery()
				{
					PublishedOnly = true,

					Keyword = keyword
				};
				var postList = await _blogRepository
					.GetPagedTagsAsync(postQuery,pageNumber,pageSize);
				ViewBag.PostQuyery = postQuery;

				return View(postList);
			}
		}
		public IActionResult About()=>View();

		public IActionResult Contact() => View();

		public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

		private readonly IBlogRepository _blogRepository;
		public BlogController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}
	}
}