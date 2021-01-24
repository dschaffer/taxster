using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxJarTaxCalculator.Models;
using TaxService;
using TaxService.Controllers;
using TaxService.Models;

namespace TaxService.Tests.Controllers
{
    [TestClass]
    public class TaxServiceControllerTest
    {
        [TestMethod]
        public void Checkout()
        {
            // Arrange
            TaxServiceController controller = new TaxServiceController();

            // Act

            // Verify initial form loads
            ViewResult result = controller.Checkout() as ViewResult;
            // Verify page loads with results
            ViewResult resultPost = controller.Checkout(100, "NY", "10590", false) as ViewResult;
            // Verify page loads after resetting form
            ViewResult resultPageReset = controller.Checkout(0, "", "", true) as ViewResult;
            // Verify error messages populate
            var model = (TaxResultViewModel)result.Model;
            var modelPost = (TaxResultViewModel)resultPost.Model;
            var modelPageReset = (TaxResultViewModel)resultPageReset.Model;

            // Assert

            // Initial form
            Assert.IsNotNull(result);
            // Page with results
            Assert.IsNotNull(resultPost);
            // Page after form reset
            Assert.IsNotNull(resultPageReset);
            // Error messages
            Assert.IsNotNull(model.ErrorMessage);
            Assert.IsNotNull(modelPost.ErrorMessage);
            Assert.IsNotNull(modelPageReset.ErrorMessage);
        }
        [TestMethod]
        public void GetTaxRateForLocation()
        {
            // Arrange
            TaxServiceController controller = new TaxServiceController();

            // Act

            // Verify tax rate is successfully returned with valid query
            Decimal result = controller.GetTaxRateForLocation(new TaxRequest()
            {
                FromCountry = "US", 
                FromState = "NY", 
                FromCity = "New York", 
                FromZip = "10003", 
                Amount = 100, 
                ShippingAmount = 0, 
                ToCountry = "US", 
                ToState = "CT",
                ToZip = "06877", 
                NexusAddresses= new List<NexusAddress>()
                {
                    new NexusAddress
                    {
                        Country = "US", 
                        State = "NY", 
                        ZipCode = "10003"
                    }, 
                    new NexusAddress
                    {
                        Country = "US", 
                        State = "CT", 
                        ZipCode = "06877"
                    }
                }
            });

            // Assert

            // Valid query
            Assert.IsNotNull(result);
            // Value returned is a decimal greater than or equal to 0
            Assert.IsTrue(result.GetType() == typeof(decimal) && result >= 0);
        }
        [TestMethod]
        public void GetTaxResultForOrder()
        {
            // Arrange
            TaxServiceController controller = new TaxServiceController();

            // Act

            // Verify tax rate is successfully returned with valid query
            TaxResult result = controller.GetTaxResultForOrder(new TaxRequest()
            {
                FromCountry = "US",
                FromState = "NY",
                FromCity = "New York",
                FromZip = "10003",
                Amount = 100,
                ShippingAmount = 0,
                ToCountry = "US",
                ToState = "CT",
                ToZip = "06877",
                NexusAddresses = new List<NexusAddress>()
                {
                    new NexusAddress
                    {
                        Country = "US",
                        State = "NY",
                        ZipCode = "10003"
                    },
                    new NexusAddress
                    {
                        Country = "US",
                        State = "CT",
                        ZipCode = "06877"
                    }
                }
            });

            // Assert

            // Valid query
            Assert.IsNotNull(result);
            // Object returned is of type TaxResult
            Assert.IsTrue(result.GetType() == typeof(TaxResult));
        }
    }
}
