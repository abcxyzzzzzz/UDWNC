using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation.Results;

namespace TatBlog.WebApi.Models;

public class ApiResponse
{
	public bool IsSuccess => Errors.Count == 0;
	public HttpStatusCode StatusCode { get; init; }
	public IList<string> Errors { get; init; }
	protected ApiResponse()
	{
		StatusCode = HttpStatusCode.OK;
		Errors = new List<string>();
	}
	public static ApiRespones<T> Success<T>(
		T result,
		HttpStatusCode statusCode = HttpStatusCode.OK)
	{
		return new ApiRespones<T>
		{
			Result = result,
			StatusCode = statusCode
		};
	}
	public static ApiRespones<T> FailWithResult<T>(
		HttpStatusCode statusCode,
		T result,
		params string[] errorMessages)
	{
		return new ApiRespones<T>()
		{
			Result = result,
			StatusCode = statusCode,
			Errors = new List<string>(errorMessages)
		};
	}
	public static ApiResponse Fail(
		HttpStatusCode statusCode,
		params string[] errorMessages)
	{
		if (errorMessages == null || errorMessages.Length == 0)
		{
			throw new ArgumentNullException(nameof(errorMessages));
		}
		return new ApiResponse()
		{
			StatusCode = statusCode,
			Errors = new List<string>(errorMessages)
		};
	}

	public static ApiResponse Fail(
		HttpStatusCode statusCode,
		ValidationResult validationResult)
	{
		return Fail(statusCode, validationResult.Errors
			.Select(x => x.ErrorMessage)
			.Where(e => !string.IsNullOrWhiteSpace(e))
			.ToArray());
	}
}
public class ApiRespones<T> : ApiResponse
{
	public T Result { get; set; }
}
