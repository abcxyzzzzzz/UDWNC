//using Microsoft.EntityFrameworkCore;
//using TatBlog.Data.Contexts;
//using TatBlog.Data.Seeder;
//using TatBlog.Data.Seeders;
//using TatBlog.Services.Blogs;

//var builder = WebApplication.CreateBuilder(args);
//{
//	builder.Services.AddControllersWithViews();
//	builder.Services.AddDbContext<BlogDbContext>(options =>
//		options.UseSqlServer(
//			builder.Configuration.GetConnectionString("DefaultConnection")));

//	builder.Services.AddScoped<IBlogRepository, BlogRepository>();
//	builder.Services.AddScoped<IDataSeeder, DataSeeder>();
//}

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//	if (app.Environment.IsDevelopment())
//	{
//		app.UseDeveloperExceptionPage();
//	}
//	app.UseExceptionHandler("/Home/Error");
//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//	app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Blog}/{action=Index}/{id?}");
//app.MapControllerRoute(
//	name: "posts-by-category",
//	pattern: "blog/category/{slug}",
//	defaults: new { controller = "Blog", action = "Category" });
//app.MapControllerRoute(
//	name: "posts-by-tag",
//	pattern: "blog/tag/{slug}",
//	defaults: new { controller = "Blog", action = "Tag" });

//app.MapControllerRoute(
//	name: "single-post",
//	pattern: "blog/post/{year:int}/{mouth:int}/{day:int}/{slug}",
//	defaults: new { controller = "Blog", action = "Post" });

//using (var scope = app.Services.CreateScope())
//{
//	var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
//	seeder.Initialize();
//}
//app.Run();

using TatBlog.WebApp.Extensions;
var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureMvc()
		.ConfigureServices();
}

var app = builder.Build();
{
	app.UseRequesrPipeline();
	app.UseBlogRoutes();
	app.UseDataSeeder();
}
app.Run();
