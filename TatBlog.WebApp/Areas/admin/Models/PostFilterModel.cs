using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApp.Areas.admin.Models;

public class PostFilterModel
{
	[DisplayName("Từ khóa")]
	public string Keyword { get; set; }
	[DisplayName("Tác giả")]
	public int? AuthorId { get; set; }
	[DisplayName("Chủ đề")]
	public int? CategoryID { get; set; }
	[DisplayName("Năm")]
	public int? Year { get; set; }
	[DisplayName("Tháng")]
	public int? Month { get; set; }

	public IEnumerable<SelectListItem> AuthorList { get; set; }
	public IEnumerable<SelectListItem> CategoryList { get; set; }
	public IEnumerable<SelectListItem> MounthList { get; set; }

	public PostFilterModel()
	{
		MounthList = Enumerable.Range(1, 12)
			.Select(m => new SelectListItem()
			{
				Value = m.ToString(),
				Text = CultureInfo.CurrentCulture
					.DateTimeFormat.GetMonthName(m)
			})
			.ToList();
	}

}
