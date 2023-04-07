namespace TatBlog.Services.Blogs
{
	public class PostQuery
	{
		public int AuthorId;
        public int PostId { get; set; }
        public int Id { get; set; }
        public bool PublishedOnly { get;  set; }
		public bool NotPublished { get;  set; }
		public int CategoryId { get;  set; }
		public string CategorySlug { get;  set; }
		public string AuthorSlug { get;  set; }
		public string TagSlug { get;  set; }
		public string Keyword { get;  set; }
		public int Year { get;  set; }
		public int Month { get;  set; }
		public string TitleSlug { get;  set; }
		public string Slug { get; set; }
		public DateTime PostedDate { get; set; }
		public DateTime TimeCreated { get; set; }
		public string Tag { get; set; }
	}
}