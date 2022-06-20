﻿using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Magento.RestClient.Modules.Catalog.Models.Products
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ProductMediaType
	{
		[EnumMember(Value = "image")] Image
	}
}