
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
	 Task<IPagedList<Post>> GetPagePostsAsync(
		 PostQuery condition, int pageNumber = 1,
		 int pageSize = 10, 
		 CancellationToken cancellationToken = default);
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
	Task<Tag> GetTagSlug(
		string slug,
	CancellationToken cancellationToken = default);
	Task<IList<TagItem>> GetTagsAsync(
		CancellationToken cancellationToken = default);
	Task<IPagedList<T>> GetPagedPostsAsync<T>(
		PostQuery condition,
		IPagingParams pagingParams,
		Func<IQueryable<Post>, IQueryable<T>> mapper);
	Task<IPagedList<Post>> GetPagedPostsAsync(
		PostQuery condition,
		int pageNumber ,
		int pageSize ,
		CancellationToken cancellationToken = default);

	Task<Post> GetPostByIdAsync(
		int postId, bool includeDetails = false,
		CancellationToken cancellationToken = default);
	Task<Post> CreateOrUpdatePostAsync(
	   Post post, IEnumerable<string> tags,
	   CancellationToken cancellationToken = default);
	Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);
	Task<IList<CategoryItem>> GetCategoriesAsync(
		bool showOnMenu = false,
		CancellationToken cancellationToken = default);
}	
