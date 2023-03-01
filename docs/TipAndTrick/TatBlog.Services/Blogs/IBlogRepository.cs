
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
	Task<Post> GetPostAnsyns(
		int year,
		int month,
		string slug,
		CancellationToken cancellationToken = default);
	Task<IList<Post>> GetPopularArticAsync(
		int numPosts,
		CancellationToken cancellationToken = default);
	Task<bool> IsPostSlugExistedAsync(
		int postID, string slug,
		CancellationToken cancellationToken = default);

	Task IncreaseWiewCountAsync(
		int postID,
		CancellationToken cancellationToken = default);
	Task<IList<CategoryItem>> GetCategoriesAsyns(
		bool showOnMenu = false,
		CancellationToken cancellationToken = default);
	Task<IPagedList<TagItem>> GetPagedTagsAsync(
		IPagingParams pagingParams,
		CancellationToken cancellationToken = default);
}
