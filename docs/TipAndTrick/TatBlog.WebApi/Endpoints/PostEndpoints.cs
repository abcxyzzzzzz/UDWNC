using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PostsBlog.Services.Blogs;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
//using TatBlog.Service.Blogs;
//using TatBlog.Service.Media;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;
using TatBlog.WebApp.Mapsters;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            //routeGroupBuilder.MapGet("/", GetPosts)
            //    .WithName("GetPosts")
            //    .Produces<PaginationResult<PostQuery>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
                .WithName("GetPostsByPostId")
                .Produces<PostItem>()
                .Produces(404);

            routeGroupBuilder.MapGet(
                "/{slug:regex(^[a-z0-9_-]+$)}/posts",
                GetPostsBySlug)
                .WithName("GetPostsByPostSlug")
                .Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddNewPost")
                .Produces(201)
                .Produces(400)
                .Produces(409);

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetPostPicture)
               .WithName("SetPostPicture")
               .Accepts<IFormFile>("multipart/form-data")
               .Produces<string>()
               .Produces(400);

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
               .WithName("UpdateAnPost")
               .Produces(204)
               .Produces(400)
               .Produces(409);
            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
               .WithName("DeleteAnPost")
               .Produces(204)
               .Produces(404);
            return app;
        }

        //private static async Task<IResult> GetPosts(
        //    [AsParameters] PostFilterModel model,
        //    IPostRepository postRepository,
        //      IMapper mapper)
        //{
        //    var postQuery = mapper.Map<PostQuery>(model);
        //    var postsList = await postRepository.GetPagedPostAsync(
        //      postQuery, model,
        //      posts => posts.ProjectToType<PostDto>());

        //    var paginationResult = new PaginationResult<PostDto>(postsList);

        //    return Results.Ok(paginationResult);
        //}

        private static async Task<IResult> GetPostDetails(
            int id,
            IPostRepository postRepository,
            IMapper mapper)
        {
            var post = await postRepository.GetCachedPostByIdAsync(id);
            return post == null
                ? Results.NotFound($"khong tim thay bai viet co ma so {id}")
                : Results.Ok(mapper.Map<PostItem>(post));
        }

        //private static async Task<IResult> GetPostById(
        //    int id,
        //    [AsParameters] PagingModel pagingModel,
        //    IPostRepository postRepository)
            
        //{
        //    var postQuery = new PostQuery()
        //    {
        //        Id = id,
        //        PublishedOnly = true,
        //    };
        //    var postsList = await postRepository.GetPostByIdAsync(postQuery,id);

        //    var paginationResult = new PaginationResult<PostDto>(postsList);
        //    return Results.Ok(paginationResult);
        //}

        private static async Task<IResult> GetPostsBySlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IPostRepository postRepository,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                Slug = slug,
                PublishedOnly = true,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(paginationResult);
        }



        private static async Task<IResult> AddPost(
            PostEditModel model,
            IValidator<PostEditModel> validator,
            IPostRepository postRepository,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(
                    validationResult.Errors.ToResponse());
            }
            if (await postRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Conflict($"Slug'{model.UrlSlug}' da duoc su dung");
            }
            var post = mapper.Map<Post>(model);
            await postRepository.AddOrUpdateAsync(post);
            return Results.CreatedAtRoute(
                "GetPostById", new { post.Id },
                mapper.Map<PostItem>(post));

        }

        private static async Task<IResult> SetPostPicture(
            int id, IFormFile imageFile,
            IPostRepository postRepository,
            IBlogRepository blogRepository,
            IAuthorRepository authorRepository,
            IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(),
                imageFile.FileName, imageFile.ContentType);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.BadRequest("khong luu duoc tap tin");
            }
            await authorRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(imageUrl);
        }

        private static async Task<IResult> UpdatePost(
            int id, PostEditModel model,
            IValidator<PostEditModel> validator,
            IPostRepository postRepository,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(
                    validationResult.Errors.ToResponse());
            }

            if (await postRepository
                .IsPostSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Conflict(
                    $"Slug '{model.UrlSlug}' da duoc su dung");
            }
            var post = mapper.Map<Post>(model);
            post.Id = id;

            return await postRepository.AddOrUpdateAsync(post)
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> DeletePost(
            int id, IPostRepository postRepository)
        {
            return await postRepository.DeletePostAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"could not find post with id={id}");
        }
        private static async Task<IResult> GetFilter(
            IAuthorRepository authorRepository,
            IBlogRepository blogRepository)
        {
            var model = new PostFilterModel()
            {
                AuthorList = (await authorRepository.GetAuthorsAsync())
                .Select(a => new SelectListItem()
                {
                    Text = a.FullName,
                    Value = a.Id.ToString()
                }),
                CategoryList = (await blogRepository.GetCategoriesAsync())
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };
            return Results.Ok(ApiResponse.Success(model));
        }
        private static async Task<IResult> GetFilteredPosts(
            [AsParameters] PostFilterModel model,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                Keyword = model.Keyword,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                Year = model.Year,
                Month = model.Month,
            };
            var postList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());
            var pagingnationResult = new PaginationResult<PostDto>(postList);

            return Results.Ok(ApiResponse.Success(pagingnationResult));
        }
    }

}






