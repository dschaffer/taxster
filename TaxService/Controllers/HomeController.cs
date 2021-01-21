using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Net.Http;
using TaxService.Models;
using System.IO;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaxService.Controllers
{
    public class HomeController : Controller
    {
        public void Index()
        {
            Response.Redirect("~/Home/Checkout");
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            TaxResultViewModel taxResultViewModel = new TaxResultViewModel();
            taxResultViewModel.ErrorMessage = "Please correct the highlighted fields and try again.";

            return View(taxResultViewModel);
        }

        [HttpPost]
        public ActionResult Checkout(decimal orderTotal, string state, string zipCode, bool resetFlag = false)
        {
            TaxResultViewModel taxResultViewModel = new TaxResultViewModel();

            if (resetFlag)
            {
                taxResultViewModel.ErrorMessage = "Please correct the highlighted fields and try again.";

                return View(taxResultViewModel);
            }

            TaxRequest taxRequest = new TaxRequest();

            // Assuming all tax calculations are based off of this address
            // This can be made configurable with additional form fields in the view 
            taxRequest.FromCity = "New York";
            taxRequest.FromCountry = "US";
            taxRequest.FromState = "NY";
            // taxRequest.FromStreet = "20 Cooper Square";
            taxRequest.FromZip = "10003";

            taxRequest.Amount = orderTotal;

            // Special Promotion: FREE SHIPPING on all orders winter 2021!
            taxRequest.ShippingAmount = 5.49M;

            taxRequest.ToCountry = "US";
            taxRequest.ToState = state;
            taxRequest.ToZip = zipCode;

            // Add From Address to Nexus Addresses
            taxRequest.NexusAddresses.Add(new NexusAddress()
            {
                Country = taxRequest.FromCountry, 
                State = taxRequest.FromState, 
                ZipCode = taxRequest.FromZip
            });

            // Add To Address to Nexus Addresses
            taxRequest.NexusAddresses.Add(new NexusAddress()
            {
                Country = taxRequest.ToCountry,
                State = taxRequest.ToState,
                ZipCode = taxRequest.ToZip
            });

            taxResultViewModel.OrderTotal = orderTotal;
            taxResultViewModel.State = state;
            taxResultViewModel.ZipCode = zipCode;
            taxResultViewModel.TaxResult = GetTaxResultForOrder(taxRequest);
            taxResultViewModel.OrderTotalWithTax = orderTotal + taxResultViewModel.TaxResult.TaxAmount;
            taxResultViewModel.ErrorMessage = "Sorry but there was a problem finding your tax rate, please try again later.";

            return View(taxResultViewModel);
        }

        public Decimal GetTaxRateForLocation(TaxRequest taxRequest)
        {
            TaxResult taxResult = new TaxResult();
            var taxRate = 0.00M;

            // TODO: Tax Jar API

            var taxApiEndpoint = WebConfigurationManager.AppSettings["TaxApiEndpointUrl"];
            var taxJarApiKey = WebConfigurationManager.AppSettings["TaxJarApiKey"];
            var taxRequestJson = JsonConvert.SerializeObject(taxRequest);

            try
            {
                var client = new RestClient(taxApiEndpoint);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Token token=\"" + taxJarApiKey + "\"");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", taxRequestJson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                var data = (JObject)JsonConvert.DeserializeObject(response.Content);

                // Parse JSON Response to TaxResult class
                // taxResult = (TaxResult)JsonConvert.DeserializeObject(response.Content);

                // assign taxRate
                taxRate = data["tax"]["rate"].Value<decimal>();
                
            }
            catch
            {
                // Log Error

            }

            return taxRate;
        }

        public TaxResult GetTaxResultForOrder(TaxRequest taxRequest)
        {
            var taxRate = GetTaxRateForLocation(taxRequest);
            var taxAmount = taxRequest.Amount * taxRate;

            TaxResult taxResult = new TaxResult();

            taxResult.TaxRate = taxRate;
            taxResult.TaxAmount = taxAmount;

            return taxResult;
        }
    }
}