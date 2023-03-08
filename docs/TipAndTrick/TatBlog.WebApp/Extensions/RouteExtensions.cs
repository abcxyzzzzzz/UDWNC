namespace TatBlog.WebApp.Extensions;

public class RouteExtensions
{
	public static IEndpointRouteBuilder UseBlogRoutes(
		this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapControllerRoute(
			name: "default",
			pattern: "{controller=Blog}/{action=Index}/{id?}");
		endpoints.MapControllerRoute(
			name: "posts-by-category",
			pattern: "blog/category/{slug}",
			defaults: new { controller = "Blog", action = "Category" });
		endpoints.MapControllerRoute(
			name: "posts-by-tag",
			pattern: "blog/tag/{slug}",
			defaults: new { controller = "Blog", action = "Tag" });

		endpoints.MapControllerRoute(
			name: "single-post",
			pattern: "blog/post/{year:int}/{mouth:int}/{day:int}/{slug}",
			defaults: new { controller = "Blog", action = "Post" });

		return endpoints;
	}
}
