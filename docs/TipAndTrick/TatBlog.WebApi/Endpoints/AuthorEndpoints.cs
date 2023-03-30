using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Media;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TatBlog.WebApi.Filters;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace TatBlog.WebApi.Endpoints
{
	public static class AuthorEndpoints
	{
		public static WebApplication MapAuthorEndpoints(
			this WebApplication app)
		{
			var routerGroupBuilder = app.MapGroup("/api/authors");

			routerGroupBuilder.MapGet("/", GetAuthors)
				.WithName("GetAuthors")
				.Produces<ApiRespones<PaginationResult<AuthorItem>>>();

			routerGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
				.WithName("GetAuthorById")
				.Produces<ApiRespones<AuthorItem>>();

			routerGroupBuilder.MapGet("/{slug:regex(^[a-z0-9 -]+$)}/posts", GetPostByAuthorSlug)
				.WithName("GetPostByAuthorSlug")
				.Produces<ApiRespones<PaginationResult<PostDto>>>();
			
			routerGroupBuilder.MapPost("/", AddAuthor)
				.WithName("AddNewAuthor")
				.AddEndpointFilter<ValidatortFilter<AuthorEditModel>>()
				.Produces<ApiRespones<string>>();

			routerGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
			.WithName("SetAuthorPicture")
			.Accepts<IFormFile>("multipart/from-data")
			.Produces<ApiRespones<string>>();


			routerGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
				.WithName("UpdateAnAuthor")
				.Produces<ApiRespones<string>>();
			routerGroupBuilder.MapPost("/{id:int}", DeleteAuthor)
				.WithName("DeleteAnAuthor")
				.Produces(201)
				.Produces(400)
				.Produces(409);



			return app;
		}

		private static async Task<IResult> GetAuthors(
			[AsParameters] AuthorFilterModel model,
			IAuthorRepository authorRepository)
		{
			var authorList = await authorRepository
				.GetPagedAuthorsAsync(model, model.Name);
			var paginationResult =
				new PaginationResult<AuthorItem>(authorList);
			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetAuthorDetails(
			int id,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
			var author = await authorRepository.GetCachedAuthorByIdAsync(id);
			return author == null ? Results.Ok(ApiResponse.Fail(System.Net.HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}"))
			: Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
		}
			
			private static async Task<IResult> GetPostByAuthorId(
			int id,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
			{
			var postQuery = new PostQuery()
			{
				AuthorId = id,
				PublishedOnly = true
			};
			var postList = await blogRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());
			var pagingnationResult = new PaginationResult<PostDto>(postList);
			return Results.Ok(ApiResponse.Success(pagingModel));
		}
		public static async Task<IResult> GetPostByAuthorSlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				AuthorSlug = slug,
				PublishedOnly = true
			};
			var postList = await blogRepository.GetPagedPostsAsync(
				postQuery,pagingModel,
				posts => posts.ProjectToType<PostDto>());
			var pagingnationResult = new PaginationResult<PostDto>(postList);
			return Results.Ok(ApiResponse.Success(pagingModel));
		}
		private static async Task<IResult> AddAuthor(
			AuthorEditModel model,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
			if(await authorRepository
				.IsAuthorSlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}
			var author = mapper.Map<Author>(model);
			await authorRepository.AddOrUpdateAsync(author);

			return Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
		}
		private static async Task<IResult> SetAuthorPicture(
			int id, IFormFile imageFile,
			IAuthorRepository authorRepository,
			IMediaManager mediaManager)
		{
			var imageUrl = await mediaManager.SaveFileAsync(
				imageFile.OpenReadStream(),
				imageFile.FileName, imageFile.ContentType);
			if (string.IsNullOrWhiteSpace(imageUrl))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được ảnh"));
			}
			await authorRepository.SetImageUrlAsync(id, imageUrl);
			return Results.Ok(ApiResponse.Success(imageUrl));
		}
		private static async Task<IResult> UpdateAuthor(
			int id, AuthorEditModel model,
			IValidator<AuthorEditModel> validator,
			IAuthorRepository authorRepository,
			IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if(validationResult.IsValid) 
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
			}

			if(await authorRepository
				.IsAuthorSlugExistedAsync(id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug'{model.UrlSlug}' da duoc su dung"));
			}
			var author = mapper.Map<Author>(model);
			author.Id=id;

			return await authorRepository.AddOrUpdateAsync(author)
				?  Results.Ok(ApiResponse.Success("Author được cập nhật", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy tác giả"));
		}
		private static async Task<IResult> DeleteAuthor(
			int id, IAuthorRepository authorRepository)
		{
			return await authorRepository.DeleteAuthorAsync(id)
				? Results.Ok(ApiResponse.Success("Author được xóa", HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Khong tim thay tac gia voi id la ={id}"));
		}
	}
}
