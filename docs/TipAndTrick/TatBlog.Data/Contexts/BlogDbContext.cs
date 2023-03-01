using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Entities;
using TatBlog.Data.Mappings;

namespace TatBlog.Data.Contexts;

public class BlogDbContext : DbContext
{
	public DbSet<Author> Authors { get; set; }

	public DbSet<Category> Categories { get; set; }

	public DbSet<Post> Posts { get; set; }

	public DbSet<Tag> Tags { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(@"Data Source=TUYENONICHAN;Initial Catalog=TatBlog;
									Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;
									ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(CategoryMap).Assembly);
	}

}
