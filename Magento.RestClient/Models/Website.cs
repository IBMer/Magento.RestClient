﻿using Newtonsoft.Json;

namespace Magento.RestClient.Models
{
    public class Website
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("default_group_id")]
        public long DefaultGroupId { get; set; }
    }
}