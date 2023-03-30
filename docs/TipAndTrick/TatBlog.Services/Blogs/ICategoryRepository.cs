﻿using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface ICategoryRepository
{
	Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);
	Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
	Task<Category> GetCachedCategoryBySlugAsync(
		string slug, CancellationToken cancellationToken = default);
	Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
	Task<Category> GetCachedCategoryByIdAsync(int categoryId);
	Task<IPagedList<Category>> GetCategoryByQueryAsync(CategoryQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
	Task<IPagedList<Category>> GetCategoryByQueryAsync(CategoryQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);
	Task<IPagedList<T>> GetCategoryByQueryAsync<T>(CategoryQuery query, IPagingParams pagingParams, Func<IQueryable<Category>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);
	Task<bool> AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
	Task<bool> DeleteCategoryByIdAsync(int? id, CancellationToken cancellationToken = default);
	Task<bool> CheckCategorySlugExisted(int id, string slug, CancellationToken cancellationToken = default);
	Task ChangeCategoryStatusAsync(int id, CancellationToken cancellationToken = default);
}