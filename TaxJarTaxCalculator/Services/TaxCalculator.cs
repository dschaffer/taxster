using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using TaxJarTaxCalculator.Models;

namespace TaxJarTaxCalculator
{
    public class TaxCalculator
    {
        public TaxCalculator()
        {
        }

        public Decimal GetTaxRateForLocation(string taxApiEndpointUrl, string taxJarApiKey, string taxRequestJsonString)
        {
            TaxResult taxResult = new TaxResult();
            var taxRate = 0.00M;

            try
            {
                // Send request to TaxJar Sales Tax API
                var client = new RestClient(taxApiEndpointUrl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Token token=\"" + taxJarApiKey + "\"");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", taxRequestJsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                var data = (JObject)JsonConvert.DeserializeObject(response.Content);

                // check for error result
                // i.e. due to bad request (zipcode and state mismatch)
                if(data["tax"] == null && data["error"] != null)
                {
                    // Log Error
                    Console.Write(string.Format("Error result received from TaxJar Sales Tax API. Request: {0}", taxRequestJsonString));

                    return -1;
                }

                // assign taxRate
                taxRate = data["tax"]["rate"].Value<decimal>();

            }
            catch
            {
                // Log Error
                Console.Write(string.Format("Error trying to call TaxJar Sales Tax API. Request: {0}", taxRequestJsonString));
            }

            return taxRate;
        }
        public TaxResult GetTaxResultForOrder(decimal amount, decimal shippingAmount, string taxApiEndpointUrl, string taxJarApiKey, string taxRequestJsonString)
        {
            var taxRate = GetTaxRateForLocation(taxApiEndpointUrl, taxJarApiKey, taxRequestJsonString);
            var taxAmount = (amount + shippingAmount) * taxRate;

            TaxResult taxResult = new TaxResult();

            taxResult.TaxRate = taxRate;
            taxResult.TaxAmount = taxAmount;

            return taxResult;
        }
    }
}
