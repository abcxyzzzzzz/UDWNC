using Microsoft.AspNetCore.Builder;
using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapster;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureCors()
		.ConfigureNlog()
		.ConfigureServices()
		.ConfigureSwaggerOpenApi()
		.ConfigureMapster()
		.ConfigureFluentValidation();
}

var app = builder.Build();
{
	app.SetupRequestPipeline();
	app.MapAuthorEndpoints();
	app.MapCategoryEndpoints();
	//app.MapPostEndpoints();
	app.Run();
}