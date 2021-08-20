﻿using System.Collections.Generic;
using System.Net;
using Magento.RestClient.Domain;
using Magento.RestClient.Exceptions;
using Magento.RestClient.Models;
using Magento.RestClient.Models.Attributes;
using Magento.RestClient.Models.Products;
using Magento.RestClient.Repositories.Abstractions;
using Magento.RestClient.Search.Extensions;
using RestSharp;

namespace Magento.RestClient.Repositories
{
	internal class AttributeRepository : AbstractRepository, IAttributeRepository
	{
		private readonly IRestClient _client;

		public AttributeRepository(IRestClient client)
		{
			this._client = client;
		}


		public IEnumerable<EntityAttribute> GetProductAttributes(long attributeSetId)
		{
			var request = new RestRequest("products/attribute-sets/{id}/attributes");
			request.Method = Method.GET;
			request.AddOrUpdateParameter("id", attributeSetId, ParameterType.UrlSegment);
			request.SetScope("all");


			var response = _client.Execute<List<EntityAttribute>>(request);
			return HandleResponse(response);
		}


		public ProductAttribute Create(ProductAttribute attribute)
		{
			var request = new RestRequest("products/attributes");
			request.SetScope("all");

			request.Method = Method.POST;
			request.AddJsonBody(new {attribute});


			var response = _client.Execute<ProductAttribute>(request);
			return HandleResponse(response);
		}

		public void DeleteProductAttribute(string attributeCode)
		{
			var request = new RestRequest("products/attributes/{attributeCode}");

			request.Method = Method.DELETE;
			request.AddOrUpdateParameter("attributeCode", attributeCode, ParameterType.UrlSegment);
			request.SetScope("all");

			_client.Execute(request);
		}

		public List<Option> GetProductAttributeOptions(string attributeCode)
		{
			var request = new RestRequest("products/attributes/{attributeCode}/options");

			request.Method = Method.GET;
			request.AddOrUpdateParameter("attributeCode", attributeCode, ParameterType.UrlSegment);
			request.SetScope("all");

			var response = _client.Execute<List<Option>>(request);
			return HandleResponse(response);
		}

		public int CreateProductAttributeOption(string attributeCode, Option option)
		{
			var request = new RestRequest("products/attributes/{attributeCode}/options");

			request.Method = Method.POST;
			request.AddOrUpdateParameter("attributeCode", attributeCode, ParameterType.UrlSegment);
			request.SetScope("all");

			request.AddJsonBody(new {option});
			var response = _client.Execute<int>(request);

			if (response.IsSuccessful)
			{
				return response.Data;
			}
			else
			{
				throw MagentoException.Parse(response.Content);
			}
		}

		public ProductAttribute GetByCode(string attributeCode)
		{
			var request = new RestRequest("products/attributes/{attributeCode}");
			request.Method = Method.GET;
			request.AddOrUpdateParameter("attributeCode", attributeCode, ParameterType.UrlSegment);
			request.SetScope("all");

			var response = _client.Execute<ProductAttribute>(request);
			return HandleResponse(response);
		}

		public ProductAttribute Update(string attributeCode, ProductAttribute attribute)
		{
			var request = new RestRequest("products/attributes/{attributeCode}");
			request.AddOrUpdateParameter("attributeCode", attributeCode, ParameterType.UrlSegment);
			request.SetScope("all");

			request.Method = Method.PUT;
			request.AddJsonBody(new {attribute});


			var response = _client.Execute<ProductAttribute>(request);
			return HandleResponse(response);
		}

		public void DeleteProductAttributeOption(string attributeCode, string optionValue)
		{
			var request = new RestRequest("products/attributes/{attributeCode}/options/{optionValue}");
			request.SetScope("all");
			request.Method = Method.POST;
			request.AddOrUpdateParameter("attributeCode", attributeCode, ParameterType.UrlSegment);
			request.AddOrUpdateParameter("optionValue", optionValue, ParameterType.UrlSegment);
		}
	}

	public abstract class AbstractRepository
	{
		protected T HandleResponse<T>(IRestResponse<T> response) where T : class
		{
			if (response.IsSuccessful)
			{
				return response.Data;
			}

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}

			throw MagentoException.Parse(response.Content);
		}
	}
}