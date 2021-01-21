using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxService;
using TaxService.Controllers;

namespace TaxService.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        public void Checkout()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Checkout() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
