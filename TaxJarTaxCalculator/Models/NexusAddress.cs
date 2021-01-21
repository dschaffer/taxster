using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxJarTaxCalculator.Models
{
    public class NexusAddress
    {
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "zip")]
        public string ZipCode { get; set; }
    }
}