﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Magento.RestClient.Domain
{
    public partial class ExtensionAttributes
    {
        [JsonProperty("discounts")] public List<Discount> Discounts { get; set; }
    }
}