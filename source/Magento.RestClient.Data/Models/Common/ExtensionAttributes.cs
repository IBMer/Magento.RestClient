﻿using System.Collections.Generic;
using Magento.RestClient.Data.Models.Payments;
using Magento.RestClient.Data.Models.Shipping;
using Newtonsoft.Json;

namespace Magento.RestClient.Data.Models.Common
{
	public record ExtensionAttributes
	{
		[JsonProperty("shipping_assignments")] public List<ShippingAssignment> ShippingAssignments { get; set; }

		[JsonProperty("payment_additional_info")]
		public List<PaymentAdditionalInfo> PaymentAdditionalInfo { get; set; }

		[JsonProperty("applied_taxes")] public List<dynamic> AppliedTaxes { get; set; }

		[JsonProperty("item_applied_taxes")] public List<dynamic> ItemAppliedTaxes { get; set; }
	}
}