using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;


namespace TatBlog.WebApi.Filters;

public class ValidatortFilter<T> : IEndpointFilter where T:class
{
	private readonly IValidator<T> _validator;

	public ValidatortFilter(IValidator<T> validator)
	{
		_validator = validator;
	}
	
	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		var model = context.Arguments
			.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;
		if(model == null)
		{
			return Results.BadRequest(
				new ValidationFailureResponse(new[]
				{
					"Khong the tao model object"
				}));
		}
		var validationResult = await _validator.ValidateAsync(model);

		if (!validationResult.IsValid)
		{
			return Results.BadRequest(validationResult.Errors.ToResponse());
		}
		return await next(context);
	}
}
