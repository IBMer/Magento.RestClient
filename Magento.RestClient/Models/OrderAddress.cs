﻿using Newtonsoft.Json;

namespace Magento.RestClient.Models
{
    public class OrderAddress : Address
    {
        [JsonProperty("address_type")] public string AddressType { get; set; }


        [JsonProperty("customer_address_id")] public long CustomerAddressId { get; set; }

        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("entity_id")] public long EntityId { get; set; }

        [JsonProperty("parent_id")] public long ParentId { get; set; }


        [JsonProperty("region_code", NullValueHandling = NullValueHandling.Ignore)]
        public string RegionCode { get; set; }
    }
}