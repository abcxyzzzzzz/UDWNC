using Akka.Actor;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using PostsBlog.Services.Blogs;
using TagBlog.Services.Blogs;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Timing;
using ITimeProvider = TatBlog.Services.Timing.ITimeProvider;

namespace TatBlog.WebApi.Extensions;

public static class Application 
{
	public static WebApplicationBuilder ConfigureServices(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddMemoryCache();
		builder.Services.AddDbContext<BlogDbContext>(option =>
		option.UseSqlServer(builder.Configuration
			.GetConnectionString("DeFaultConnection")));

		builder.Services.AddScoped<ITimeProvider, LocalTimeProvider>();
		builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
		builder.Services.AddScoped<IBlogRepository, BlogRepository>();
		builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
		builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
		builder.Services.AddScoped<IPostRepository, PostRepository>();
		return builder;
	}
	public static WebApplicationBuilder ConfigureCors(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddCors(option =>
		{
			option.AddPolicy("TatBlogApp", policyBuilder =>
				policyBuilder
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod());
		});
		return builder;
	}
	public static WebApplicationBuilder ConfigureNlog(
		this WebApplicationBuilder builder)
	{
		builder.Logging.ClearProviders();
		builder.Host.UseNLog();
		return builder;
	}
	public static WebApplicationBuilder ConfigureSwaggerOpenApi(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		return builder;
	}
	public static WebApplication SetupRequestPipeline(
		this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}
		app.UseStaticFiles();
		app.UseHttpsRedirection();
		app.UseCors("TatBlogApp");

		return app;
	}
}
