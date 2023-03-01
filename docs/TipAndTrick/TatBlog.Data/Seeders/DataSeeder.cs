using TatBlog.Data.Seeder;
using TatBlog.Data.Contexts;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Seeders
{
	public class DataSeeder : IDataSeeder
	{
		private readonly BlogDbContext _dbContext;

		public DataSeeder(BlogDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public void Initialize()
		{
			_dbContext.Database.EnsureCreated();

			if (_dbContext.Posts.Any()) return;

			var author = AddAuthors();
			var categories = AddCategories();
			var tags = AddTags();
			var posts = AddPosts(author, categories, tags);
		}

		private IList<Author> AddAuthors()
		{
			var authors = new List<Author>()
		{
			new()
			{
				FullName = "Jason",
				UrlSlug = "Jason",
				Email = "Jason@gmail.com",
				JoinedDate = new DateTime(2022,10,1)
			},
			new()
			{
				FullName = "Jessica",
				UrlSlug = "Jessica",
				Email = "Jesica@gmail.com",
				JoinedDate = new DateTime(2021,11,10)
			},
			new()
			{
				FullName = "Jessica2",
				UrlSlug = "Jessica",
				Email = "Jesica2@gmail.com",
				JoinedDate = new DateTime(2021,11,10)
			},
			new()
			{
				FullName = "Jessica3",
				UrlSlug = "Jessica",
				Email = "Jesica3@gmail.com",
				JoinedDate = new DateTime(2021,11,10)
			},
			new()
			{
				FullName = "Jessica4",
				UrlSlug = "Jessica",
				Email = "Jesica4@gmail.com",
				JoinedDate = new DateTime(2021,11,10)
			},
		};

			_dbContext.Authors.AddRange(authors);
			_dbContext.SaveChanges();

			return authors;
		}
		private IList<Category> AddCategories()
		{
			var categories = new List<Category>
		{
			new() {Name = ".NET Core", Description = ".NET Core", UrlSlug= "adsaad"},
			new() {Name = "Arichiacture", Description = "Arichiacture", UrlSlug= "adsaad"},
			new() {Name = "Messaging", Description ="Messaging", UrlSlug= "adsaad"},
			new() {Name = "OPP", Description = "Object-Or Programing", UrlSlug= "asndmbasdsa"},
			new() {Name = "Design Patterns", Description = "Design Patterns", UrlSlug= "dsadsadsadn"}
		};
			_dbContext.AddRange(categories);
			_dbContext.SaveChanges();

			return categories;
		}
		private IList<Tag> AddTags()
		{
			var tags = new List<Tag>
		{
			new() {Name = "Goole", Description = "Goo applications", UrlSlug= "abccc"},
			new() {Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug= "abccc"},
			new() {Name = "Razor Page", Description = "Razor Page", UrlSlug= "abccc"},
			new() {Name = "Blazor", Description = "Blazor", UrlSlug= "abccc"},
			new() {Name = "Deep Learning", Description = "Learning", UrlSlug= "abccc"},
			new() {Name = "Neural Network", Description = "Neural Network", UrlSlug= "abccc"},
			new() {Name = "Deep Learning", Description = "Learning", UrlSlug= "abccc"},
			new() {Name = "Deep Learning", Description = "Learning", UrlSlug= "abccc"},
			new() {Name = "Deep Learning", Description = "Learning", UrlSlug= "abccc"},
			new() {Name = "Deep Learning", Description = "Learning", UrlSlug= "abccc"},
			new() {Name = "Deep Learning", Description = "Learning", UrlSlug= "abccc"},
			new() {Name = "Deep Learning", Description = "Learning", UrlSlug= "abccc"},
			new() {Name = "Deep Learning", Description = "Learning", UrlSlug= "abccc"},
			new() {Name = "Nerual Network", Description = "Network", UrlSlug= "abccc"}
		};
			_dbContext.AddRange(tags);
			_dbContext.SaveChanges();

			return tags;
		}
		private IList<Post> AddPosts(IList<Author> author, IList<Category> categories, IList<Tag> tags)
		{
			var posts = new List<Post>()
		{
			new ()
			{
				Title = "ASP.NET Core Diagnostic Scenarios",
				ShortDescription = "David and friends has a great repos ",
				Description = "Heres a few great DON'T and DO examples",
				Meta = "David and friends has a great repository filled",
				UrlSlug="aspnet-core-diagnostic-scenarios",
				Published = true,
				PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
				ModifiedDate = null,
				ViewCount = 10,
				Author= author[0],
				Category = categories[0],

				Tags = new List<Tag>()
				{
					tags[0]
				}

			},
			new ()
			{
				Title = "ASP.NET Core Diagnostic Scenarios",
				ShortDescription = "David and friends has a great repos ",
				Description = "Heres a few great DON'T and DO examples",
				Meta = "David and friends has a great repository filled",
				UrlSlug="aspnet-core-diagnostic-scenarios",
				Published = true,
				PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
				ModifiedDate = null,
				ViewCount = 10,
				Author= author[0],
				Category = categories[0],

				Tags = new List<Tag>()
				{
					tags[1]
				}

			},
			new ()
			{
				Title = "ASP.NET Core Diagnostic Scenarios",
				ShortDescription = "David and friends has a great repos ",
				Description = "Heres a few great DON'T and DO examples",
				Meta = "David and friends has a great repository filled",
				UrlSlug="aspnet-core-diagnostic-scenarios",
				Published = true,
				PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
				ModifiedDate = null,
				ViewCount = 10,
				Author= author[0],
				Category = categories[0],

				Tags = new List<Tag>()
				{
					tags[2]
				}

			},
			new ()
			{
				Title = "ASP.NET Core Diagnostic Scenarios",
				ShortDescription = "David and friends has a great repos ",
				Description = "Heres a few great DON'T and DO examples",
				Meta = "David and friends has a great repository filled",
				UrlSlug="aspnet-core-diagnostic-scenarios",
				Published = true,
				PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
				ModifiedDate = null,
				ViewCount = 10,
				Author= author[0],
				Category = categories[0],

				Tags = new List<Tag>()
				{
					tags[3]
				}

			},
			new ()
			{
				Title = "ASP.NET Core Diagnostic Scenarios",
				ShortDescription = "David and friends has a great repos ",
				Description = "Heres a few great DON'T and DO examples",
				Meta = "David and friends has a great repository filled",
				UrlSlug="aspnet-core-diagnostic-scenarios",
				Published = true,
				PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
				ModifiedDate = null,
				ViewCount = 10,
				Author= author[0],
				Category = categories[0],

				Tags = new List<Tag>()
				{
					tags[4]
				}

			}
		};
			_dbContext.AddRange(posts);
			_dbContext.SaveChanges();

			return posts;
		}

		
	}
}
