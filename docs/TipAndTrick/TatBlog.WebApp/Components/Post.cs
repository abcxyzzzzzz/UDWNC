using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
namespace TatBlog.WebApp.Components;


public class Post : ViewComponent
{
	private readonly IBlogRepository _blogRepository;
	public Post(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}
	public async Task<IViewComponentResult> InvokeAsync()
	{
		var categories = await _blogRepository.GetPopularArticAsync(3);
		return View(categories);
	}
}
