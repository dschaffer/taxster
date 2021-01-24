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
using TaxJarTaxCalculator;
using TaxJarTaxCalculator.Models;

namespace TaxService.Controllers
{
    public class TaxServiceController : Controller
    {
        public void Index()
        {
            // Just redirect to Checkout page
            Response.Redirect("~/TaxService/Checkout");
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            // Render empty form on new page request
            TaxResultViewModel taxResultViewModel = new TaxResultViewModel();
            // Populate error message used by client side validation
            taxResultViewModel.ErrorMessage = "Please correct the highlighted fields and try again.";

            return View(taxResultViewModel);
        }
        [HttpPost]
        public ActionResult Checkout(decimal orderTotal, string state, string zipCode, bool resetFlag = false)
        {
            TaxResultViewModel taxResultViewModel = new TaxResultViewModel();

            // Reset the form to accept new data if requested by the user
            if (resetFlag)
            {
                // Populate error message used by client side validation
                taxResultViewModel.ErrorMessage = "Please correct the highlighted fields and try again.";

                return View(taxResultViewModel);
            }

            TaxRequest taxRequest = new TaxRequest();

            // Assuming the store is located at this address, so this From Address is used in all tax calculations
            // This can be made configurable with additional form fields in the view 
            taxRequest.FromCity = "New York";
            taxRequest.FromCountry = "US";
            taxRequest.FromState = "NY";
            // taxRequest.FromStreet = "20 Cooper Square";
            taxRequest.FromZip = "10003";

            taxRequest.Amount = orderTotal;

            // Special Promotion: FREE SHIPPING on all orders winter 2021!
            taxRequest.ShippingAmount = 0;

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
            // Call TaxCalculator
            taxResultViewModel.TaxResult = GetTaxResultForOrder(taxRequest);
            taxResultViewModel.OrderTotalWithTax = orderTotal + taxResultViewModel.TaxResult.TaxAmount;

            // Check for bad request
            // Likely due to state/zipcode mismatch
            if(taxResultViewModel.TaxResult.TaxRate == -1)
            {
                // Populate error message to display to user in case of bad request
                taxResultViewModel.ErrorMessage = "Sorry but there was a problem finding your tax rate, please check state and zipcode or try again later.";
                taxResultViewModel.BadRequest = true;
            }

            return View(taxResultViewModel);
        }
        public TaxResult GetTaxResultForOrder(TaxRequest taxRequest)
        {
            // API Credentials in config file
            var taxApiEndpointUrl = WebConfigurationManager.AppSettings["TaxApiEndpointUrl"];
            var taxJarApiKey = WebConfigurationManager.AppSettings["TaxJarApiKey"];
            // API expects TaxRequest as JSON string
            var taxRequestJsonString = JsonConvert.SerializeObject(taxRequest);

            TaxResult taxResult = new TaxResult();

            // Instantiate TaxJar Tax Calculator
            TaxCalculator taxJarTaxCalculator = new TaxCalculator();

            // Call TaxCalculator application
            taxResult = taxJarTaxCalculator.GetTaxResultForOrder(taxRequest.Amount, taxRequest.ShippingAmount, taxApiEndpointUrl, taxJarApiKey, taxRequestJsonString);

            return taxResult;
        }

        public Decimal GetTaxRateForLocation(TaxRequest taxRequest)
        {
            // API Credentials in config file
            var taxApiEndpointUrl = WebConfigurationManager.AppSettings["TaxApiEndpointUrl"];
            var taxJarApiKey = WebConfigurationManager.AppSettings["TaxJarApiKey"];
            // API expects TaxRequest as JSON string
            var taxRequestJsonString = JsonConvert.SerializeObject(taxRequest);

            // Instantiate TaxJar Tax Calculator
            TaxCalculator taxJarTaxCalculator = new TaxCalculator();

            // Call TaxCalculator application
            Decimal taxRateForLocation = taxJarTaxCalculator.GetTaxRateForLocation(taxApiEndpointUrl, taxJarApiKey, taxRequestJsonString);

            return taxRateForLocation;
        }
    }
}