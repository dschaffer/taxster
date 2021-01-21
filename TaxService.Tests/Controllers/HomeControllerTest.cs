using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxService;
using TaxService.Controllers;
using TaxService.Models;

namespace TaxService.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Checkout()
        {
            // Arrange
            HomeController controller = new HomeController();

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
    }
}
