using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Mapsters;

namespace PostsBlog.Services.Blogs
{
    public interface IPostRepository
    {
        Task<Post> GetPostBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);

        Task<Post> GetCachedPostBySlugAsync(
            string slug, 
            CancellationToken cancellationToken = default);

        Task<Post> GetPostByIdAsync(TatBlog.Services.Blogs.PostQuery postQuery, int PostId);

        Task<Post> GetCachedPostByIdAsync(int PostId);

        Task<IList<PostItem>> GetPostsAsync(
            CancellationToken cancellationToken = default);

        Task<IPagedList<PostItem>> GetPagedPostsAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedPostsAsync<T>(
            Func<IQueryable<Post>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(
            Post Post,
            CancellationToken cancellationToken = default);

        Task<bool> DeletePostAsync(
            int PostId,
            CancellationToken cancellationToken = default);

        Task<bool> IsPostSlugExistedAsync(
            int PostId, string slug,
            CancellationToken cancellationToken = default);


    }
}