using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeder;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);
{
	builder.Services.AddControllersWithViews();
	builder.Services.AddDbContext<BlogDbContext>(options =>
		options.UseSqlServer(
			builder.Configuration.GetConnectionString("DefaultConnection")));

	builder.Services.AddScoped<IBlogRepository, BlogRepository>();
	builder.Services.AddScoped<IDataSeeder, DataSeeder>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
	}
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Blog}/{action=Index}/{id?}");



using (var scope = app.Services.CreateScope())
{
	var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
	seeder.Initialize();
}
app.Run();


