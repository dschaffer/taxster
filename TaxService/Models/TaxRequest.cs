using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxService.Models
{
    public class TaxRequest
    {
        [JsonProperty(PropertyName = "from_country")]
        public string FromCountry { get; set; }
        [JsonProperty(PropertyName = "from_zip")]
        public string FromZip { get; set; }
        [JsonProperty(PropertyName = "from_state")]
        public string FromState { get; set; }
        [JsonProperty(PropertyName = "from_city")]
        public string FromCity { get; set; }
        [JsonProperty(PropertyName = "from_street")]
        public string FromStreet { get; set; }
        [JsonProperty(PropertyName = "to_country")]
        public string ToCountry { get; set; }
        [JsonProperty(PropertyName = "to_zip")]
        public string ToZip { get; set; }
        [JsonProperty(PropertyName = "to_state")]
        public string ToState { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "shipping")]
        public decimal ShippingAmount { get; set; }
        [JsonProperty(PropertyName = "nexus_addresses")]
        public List<NexusAddress> NexusAddresses { get; set; }

        public TaxRequest()
        {
            NexusAddresses = new List<NexusAddress>();
        }
    }
}