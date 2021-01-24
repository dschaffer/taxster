using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxJarTaxCalculator;
using TaxJarTaxCalculator.Models;

namespace TaxJarTaxCalculator.Tests.Services
{
    [TestClass]
    public class TaxCalculatorTest
    {
        [TestMethod]
        public void GetTaxRateForLocation()
        {
            // Arrange
            TaxCalculator taxCalculator = new TaxCalculator();

            // Act

            // Verify tax rate is successfully returned with valid query
            Decimal result = taxCalculator.GetTaxRateForLocation("https://api.taxjar.com/v2/taxes", "5da2f821eee4035db4771edab942a4cc", "{\"from_country\":\"US\",\"from_zip\":\"10003\",\"from_state\":\"NY\",\"from_city\":\"New York\",\"from_street\":null,\"to_country\":\"US\",\"to_zip\":\"06877\",\"to_state\":\"CT\",\"amount\":100.0,\"shipping\":0.0,\"nexus_addresses\":[{\"country\":\"US\",\"state\":\"NY\",\"zip\":\"10003\"},{\"country\":\"US\",\"state\":\"CT\",\"zip\":\"06877\"}]}");
            // Verify no error is generated when query with state/zicode mismatch
            Decimal resultStateZipMismatch = taxCalculator.GetTaxRateForLocation("https://api.taxjar.com/v2/taxes", "5da2f821eee4035db4771edab942a4cc", "{\"from_country\":\"US\",\"from_zip\":\"10003\",\"from_state\":\"NY\",\"from_city\":\"New York\",\"from_street\":null,\"to_country\":\"US\",\"to_zip\":\"10590\",\"to_state\":\"CT\",\"amount\":100.0,\"shipping\":0.0,\"nexus_addresses\":[{\"country\":\"US\",\"state\":\"NY\",\"zip\":\"10003\"},{\"country\":\"US\",\"state\":\"CT\",\"zip\":\"10590\"}]}");

            // Assert

            // Valid query
            Assert.IsNotNull(result);
            // Value returned is a decimal greater than or equal to 0
            Assert.IsTrue(result.GetType() == typeof(decimal) && result >= 0);
            // State/Zipcode mismatch
            Assert.IsNotNull(resultStateZipMismatch);
        }
        [TestMethod]
        public void GetTaxResultForOrder()
        {
            // Arrange
            TaxCalculator taxCalculator = new TaxCalculator();

            // Act

            // Verify valid TaxResult is returned to page
            TaxResult result = taxCalculator.GetTaxResultForOrder(100, 0, "https://api.taxjar.com/v2/taxes", "5da2f821eee4035db4771edab942a4cc", "{\"from_country\":\"US\",\"from_zip\":\"10003\",\"from_state\":\"NY\",\"from_city\":\"New York\",\"from_street\":null,\"to_country\":\"US\",\"to_zip\":\"06877\",\"to_state\":\"CT\",\"amount\":100.0,\"shipping\":0.0,\"nexus_addresses\":[{\"country\":\"US\",\"state\":\"NY\",\"zip\":\"10003\"},{\"country\":\"US\",\"state\":\"CT\",\"zip\":\"06877\"}]}");
            // Verify no error is generated when query with state/zicode mismatch
            TaxResult resultStateZipMismatch = taxCalculator.GetTaxResultForOrder(100, 0, "https://api.taxjar.com/v2/taxes", "5da2f821eee4035db4771edab942a4cc", "{\"from_country\":\"US\",\"from_zip\":\"10003\",\"from_state\":\"NY\",\"from_city\":\"New York\",\"from_street\":null,\"to_country\":\"US\",\"to_zip\":\"10590\",\"to_state\":\"CT\",\"amount\":100.0,\"shipping\":0.0,\"nexus_addresses\":[{\"country\":\"US\",\"state\":\"NY\",\"zip\":\"10003\"},{\"country\":\"US\",\"state\":\"CT\",\"zip\":\"10590\"}]}");

            // Assert

            // Valid TaxResult
            Assert.IsNotNull(result);
            // Object returned is of type TaxResult
            Assert.IsTrue(result.GetType() == typeof(TaxResult));
            // State/Zipcode mismatch
            Assert.IsNotNull(resultStateZipMismatch);
        }
    }
}
