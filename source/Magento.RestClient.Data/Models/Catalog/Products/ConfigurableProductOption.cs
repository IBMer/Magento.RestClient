﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Magento.RestClient.Data.Models.Catalog.Products
{
	public record ConfigurableProductOption
	{
		[JsonProperty("id")] public long Id { get; set; }

		[JsonProperty("attribute_id")] public long AttributeId { get; set; }

		[JsonProperty("label")] public string Label { get; set; }

		[JsonProperty("position")] public long Position { get; set; }

		[JsonProperty("values")] public List<ConfigurableProductValue> Values { get; set; }

		[JsonProperty("product_id")] public long ProductId { get; set; }
	}
}