using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
	private readonly BlogDbContext _context;

	public BlogRepository(BlogDbContext context)
	{
		_context = context;
	}

	public async Task<IList<CategoryItem>> GetCategoriesAsyns(bool showOnMenu = false, CancellationToken cancellationToken = default)
	{
		IQueryable<Category> categories = _context.Set<Category>();
		if (showOnMenu)
		{
			categories = categories.Where(x => x.ShowOnMenu);
		}
		return await categories
			.OrderBy(x => x.Name)
			.Select(x => new CategoryItem()
			{
				
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				ShowOnMenu = x.ShowOnMenu,
				PostCount = x.Posts.Count(p => p.Published)

			})
			.ToListAsync(cancellationToken);
	}


	public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
	{
		var tagQuery = _context.Set<Tag>()
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			});

		return await tagQuery
			.ToPagedListAsync(pagingParams,cancellationToken);
	}

	public async Task<IList<Post>> GetPopularArticAsync(int numPosts, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.Include(x => x.Author)
			.Include(x => x.Category)
			.OrderByDescending(x => x.ViewCount)
			.Take(numPosts)
			.ToListAsync(cancellationToken);
	}

	public async Task<Post> GetPostAnsyns(int year, int month, string slug, CancellationToken cancellationToken = default)
	{
		IQueryable<Post> postsQuery = _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Author);
		if (year > 0)
		{
			postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
		}
		if (month > 0)
		{
			postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
		}
		if (!string.IsNullOrWhiteSpace(slug))
		{
			postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
		}
		return await postsQuery.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default)
	{
		IQueryable<Tag> tags = _context.Set<Tag>();
		return await tags
			.OrderBy(x => x.Name)
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
	}

	public async Task<Tag> GetTagSlug(string slug, CancellationToken cancellationToken = default)
	{
		if(string.IsNullOrWhiteSpace(slug)) return null;
		return await _context.Set<Tag>().FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
	}

	public async Task IncreaseWiewCountAsync(int postID, CancellationToken cancellationToken = default)
	{
		await _context.Set<Post>()
		.Where(x => x.Id == postID)
		.ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
			cancellationToken);
	}

	public async Task<bool> IsPostSlugExistedAsync(int postID, string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.AnyAsync(x => x.Id != postID && x.UrlSlug == slug,
			cancellationToken);
	}
}
