using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
namespace TatBlog.WebApp.Components;


public class PostRandom : ViewComponent
{
	private readonly IBlogRepository _blogRepository;
	public PostRandom(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}
	public async Task<IViewComponentResult> InvokeAsync()
	{
		var categories = await _blogRepository.GetPostByIdAsync(5);
		return View(categories);
	}
}
