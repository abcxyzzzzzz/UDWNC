using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TatBlog.Services.Blogs;
using TatBlog.Core.DTO;
using TatBlog.WebApp.Models;

namespace TatBlog.WebApp.Controllers
{
	public class BlogController : Controller
	{

		private readonly IBlogRepository _blogRepository;
		public BlogController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}
		public async Task<IActionResult> Index(
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "k")] string keywork = null,
		[FromQuery(Name = "ps")] int pageSize = 10)

		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				Keyword = keywork,

			};
			var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.PostQuyery = postQuery;

			return View(postList);
		}
		public async Task<IActionResult> Category(
			[FromQuery(Name = "slug")] string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)

		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				CategorySlug = slug,

			};
			var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.PostQuyery = postQuery;

			return View(postList);
		}
		public async Task<IActionResult> Author(
			[FromQuery(Name = "slug")] string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)

		{
			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				AuthorSlug = slug,

			};
			var postList = await _blogRepository.GetPagePostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.PostQuyery = postQuery;

			return View(postList);
		}
		public IActionResult About() => View();

		public IActionResult Contact() => View();

		public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

	}
}