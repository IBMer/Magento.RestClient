﻿using System;
using Magento.RestClient.Data.Models.Category;
using Magento.RestClient.Data.Repositories.Abstractions;
using Magento.RestClient.Domain.Models;

namespace Magento.RestClient.Domain.Extensions
{
	public static class CategoryExtensions
	{
		public static CategoryModel ToModel(this Category category, IAdminContext context)
		{
			if (category.Id != 0)
			{
				return new CategoryModel(context, category.Id);
			}

			throw new ArgumentNullException(nameof(category.Id));
		}
	}
}