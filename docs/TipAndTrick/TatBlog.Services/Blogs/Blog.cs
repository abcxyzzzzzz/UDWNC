using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tagblog.Services.Blogs;

public static class Blog
{
	public static IEnumerable<string> SplitComelCase(this string input)
	{
		return Regex.Split(input, @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str));
	}
	public static string Firstchuruppercase(this string input)
	{
		return $"{char.ToUpper(input[0])}{input.Substring(1)}";
	}

	public static string GenerateSlug(this string slug)
	{

		var splittoValidFormat = slug.Split(new[] { " ", ",", ";", ".", "-", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < splittoValidFormat.Length; i++)
		{
			splittoValidFormat[i] = splittoValidFormat[i].Firstchuruppercase();
		}
		var refixAlphabet = splittoValidFormat;
		var slugFormat = string.Join("", refixAlphabet);

		var reflectionSlug = string.Join("-", slugFormat.SplitComelCase());
		return reflectionSlug.ToLower();
	}
}
