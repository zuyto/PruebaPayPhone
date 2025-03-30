// <copyright file="ValidationFilterTests.cs" company="Mauro Martinez">
// 	Copyright (c) 
// 	All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// 	See License.txt in the project root for license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using WALLET_SERVICE.Api.Filters;

namespace WALLET_SERVICE.UnitTests.Api.Filters
{
	public class ValidationFilterTests
	{
		[Fact]
		public async Task OnActionExecutionAsync()
		{
			// Arrange
			var validationFilter = new ValidationFilter();
			var context = new ActionExecutingContext(
				new ActionContext
				{
					HttpContext = new DefaultHttpContext(),
					RouteData = new RouteData(),
					ActionDescriptor = new ActionDescriptor(),
				},
				new List<IFilterMetadata>(),
				new Dictionary<string, object?>(),
				new object());
			var next = new ActionExecutionDelegate(() =>
			{
				return Task.FromResult(new ActionExecutedContext(
					new ActionContext
					{
						HttpContext = new DefaultHttpContext(),
						RouteData = new RouteData(),
						ActionDescriptor = new ActionDescriptor(),
					},
					new List<IFilterMetadata>(),
					new object()));
			});

			// Act
			await validationFilter.OnActionExecutionAsync(context, next);

			// Assert
			Assert.NotNull(context);
		}
		[Fact]
		public async Task OnActionExecutionAsync_InvalidModelState()
		{
			// Arrange
			var validationFilter = new ValidationFilter();
			var context = new ActionExecutingContext(
							new ActionContext
							{
								HttpContext = new DefaultHttpContext(),
								RouteData = new RouteData(),
								ActionDescriptor = new ActionDescriptor(),
							},
										new List<IFilterMetadata>(),
													new Dictionary<string, object?>(),
																new object());
			context.ModelState.AddModelError("key", "error message");
			var next = new ActionExecutionDelegate(() =>
			{
				return Task.FromResult(new ActionExecutedContext(
									new ActionContext
									{
										HttpContext = new DefaultHttpContext(),
										RouteData = new RouteData(),
										ActionDescriptor = new ActionDescriptor(),
									},
													new List<IFilterMetadata>(),
																	new object()));
			});

			// Act
			await validationFilter.OnActionExecutionAsync(context, next);

			// Assert
			Assert.NotNull(context.Result);
		}
	}
}
