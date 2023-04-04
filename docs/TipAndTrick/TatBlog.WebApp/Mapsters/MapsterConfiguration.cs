﻿using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Mapsters;

public class MapsterConfiguration : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Post, PostItem>()
			.Map(dest => dest.CategoryName, src => src.Category.Name)
			.Map(dest => dest.Tags, src => src.Tags.Select(x => x.Name));
		config.NewConfig<PostFilterModel, PostQuery>()
			.Map(dest => dest.PublishedOnly, src => false);

		config.NewConfig<PostEditModel,Post>()
			.Ignore(dest => dest.Id)
			.Ignore(dest =>dest.ImageUrl);

		config.NewConfig<Post, PostEditModel>()
			.Map(dest=>dest.SelectedTags, src=>
				string.Join("\r\n", src.Tags.Select(x=>x.Name)))
			.Ignore(dest => dest.CategoryList)
			.Ignore(dest => dest.AuthorList)
			.Ignore(dest => dest.ImageFile);         
	}
}