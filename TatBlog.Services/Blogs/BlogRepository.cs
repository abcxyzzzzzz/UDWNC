﻿using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tagblog.Services.Blogs;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;
using Category = TatBlog.Core.Entities.Category;
using Tag = TatBlog.Core.Entities.Tag;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
	private readonly BlogDbContext _context;

	public BlogRepository(BlogDbContext context)
	{
		_context = context;
	}

	public async Task<IList<CategoryItem>> GetCategoriesAsync(
		bool showOnMenu = false,
		CancellationToken cancellationToken = default)
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
	public async Task<Post> GetPostByIdAsync(
		int postId, bool includeDetails = false,
		CancellationToken cancellationToken = default)
	{
		if (!includeDetails)
		{
			return await _context.Set<Post>().FindAsync(postId);
		}

		return await _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Author)
			.Include(x => x.Tags)
			.FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
	}

	public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>()
			.OrderBy(a => a.FullName)
			.Select(a => new AuthorItem()
			{
				Id = a.Id,
				FullName = a.FullName,
				Email = a.ToString(),
				JoinedDay = a.JoinedDate,
				ImageUrl = a.ImageUrl,
				UrlSlug = a.UrlSlug,
				Notes = a.Notes,
				PostCount = a.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
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
		if (string.IsNullOrWhiteSpace(slug)) return null;
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
	public async Task<IPagedList<Post>> GetPagedPostsAsync(
	PostQuery condition,
	int pageNumber = 1,
	int pageSize = 10,
	CancellationToken cancellationToken = default)
	{
		return await FilterPosts(condition).ToPagedListAsync(
			pageNumber, pageSize,
			nameof(Post.PostedDate), "DESC",
			cancellationToken);
	}
	public async Task<Post> CreateOrUpdatePostAsync(
		Post post, IEnumerable<string> tags,
		CancellationToken cancellationToken = default)
	{
		if (post.Id > 0)
		{
			await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
		}
		else
		{
			post.Tags = new List<Tag>();
		}

		var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => new
			{
				Name = x,
				Slug = x.GenerateSlug()
			})
			.GroupBy(x => x.Slug)
			.ToDictionary(g => g.Key, g => g.First().Name);


		foreach (var kv in validTags)
		{
			if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

			var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
			{
				Name = kv.Value,
				Description = kv.Value,
				UrlSlug = kv.Key
			};

			post.Tags.Add(tag);
		}

		post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

		if (post.Id > 0)
			_context.Update(post);
		else
			_context.Add(post);

		await _context.SaveChangesAsync(cancellationToken);

		return post;
	}

	private async Task<Tag> GetTagAsync(string slug, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(slug)) return null;

		return await _context.Set<Tag>()
			.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
	}

	private IQueryable<Post> FilterPosts(PostQuery condition)
	{
		IQueryable<Post> posts = _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Author)
			.Include(x => x.Tags);

		if (condition.PublishedOnly)
		{
			posts = posts.Where(x => x.Published);
		}

		if (condition.NotPublished)
		{
			posts = posts.Where(x => !x.Published);
		}

		if (condition.CategoryId > 0)
		{
			posts = posts.Where(x => x.CategoryId == condition.CategoryId);
		}

		if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
		{
			posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
		}

		if (condition.AuthorId > 0)
		{
			posts = posts.Where(x => x.AuthorId == condition.AuthorId);
		}

		if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
		{
			posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
		}

		if (!string.IsNullOrWhiteSpace(condition.TagSlug))
		{
			posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
		}

		if (!string.IsNullOrWhiteSpace(condition.Keyword))
		{
			posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
									 x.ShortDescription.Contains(condition.Keyword) ||
									 x.Description.Contains(condition.Keyword) ||
									 x.Category.Name.Contains(condition.Keyword) ||
									 x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
		}

		if (condition.Year > 0)
		{
			posts = posts.Where(x => x.PostedDate.Year == condition.Year);
		}

		if (condition.Month > 0)
		{
			posts = posts.Where(x => x.PostedDate.Month == condition.Month);
		}

		if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
		{
			posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
		}

		return posts;
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
		return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<IPagedList<Post>> GetPagePostsAsync(PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
	{
		return await FilterPosts(condition).ToPagedListAsync(
		   pageNumber, pageSize, nameof(Post.PostedDate), "DESC", cancellationToken);
	}
}