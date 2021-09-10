﻿using System;
using RestSharp;

namespace Magento.RestClient.Search.Extensions
{
	public static class RestRequestExtensions
	{
	
		public static IRestRequest SetScope(this IRestRequest request, string scope)
		{
			request.AddOrUpdateParameter("scope", scope, ParameterType.UrlSegment);
			return request;
		}
	}
}