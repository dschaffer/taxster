# Taxster
A Tax Service

Release 1

Taxter is an application that calculates and displays a tax rate, tax amount and new total with tax given a specific purchase amount and location the purchase is taking place. The application consists of 2 main parts, a Tax Calculator and a Tax Service, and associated Test Projects.

## Key Assumptions

1. The form currently only supports US addresses and currency. Support for internation addresses and aditional currencies could be added in the future.
2. Additional TaxCalculator classes using other Tax APIs can be added and made configurable in the future.
3. The From Address is static and set to an address in New York, but the From Address could be made configurable in the future. The API seems to calculate the taxes based off of the To Address and not the From Address.
4. Shipping is FREE currently due to a marketing promotion.

------------------------------------------------------------------

## TaxCalculator
The TaxCalculator class consists of a constructor and 2 main methods, and uses 3 models.

### NexusAddress.cs
Represents a NexusAddress with associated JSON property names. A list of NexusAddresses, representing the To and From addresses, is sent in every request to the TaxJar Sales Tax API.

### TaxRequest.cs
Represents a TaxRequest with associated JSON property names. Every request to the TaxJar Sales Tax API is sent as a TaxRequest object. It contains all address information, the subtotal amount and shipping amount.

### TaxResult.cs
Represents a TaxResult used by the Checkout page in the Tax Service. It contains the tax rate and subtotal with tax as decimal numbers.

### GetTaxRateForLocation
Takes API credentials and a TaxRequest as a JSON string and sends a request to the TaxJar Sales Tax API. After a successful request and response it returns a tax rate as a decimal number. -1 is returned when an error result is returned from the API and an error is written to the console. If request fails an error is written to the console.

### GetTaxResultForOrder
Takes subtotal and shipping amounts, API credentials and a TaxRequest as a JSON string and calls GetTaxRateForLocation. Calculates a tax amount and returns a TaxResult object.

### Test Project
TaxJarTaxCalculator.Tests contains 2 test methods:

1. GetTaxRateForLocation - tests that a decimal number greater than 0 is returned when supplied with a valid address and purchase amount.
1. GetTaxResultForOrder - tests that a valid object of type TaxResult is returned when supplied with a valid address and purchase amount.


------------------------------------------------------------------

## TaxService
The TaxService application consists of one model, one controller and one view. It currently only supports US addresses and currency. It uses a tax calculator powered by the TaxJar Sales Tax API to calculate tax rates but in the future can be expanded to use additional configurable tax calculators.

### TaxResultViewModel.cs
The TaxResultViewModel contains:

1. A TaxResult object
1. Properties used to populate the form on the Checkout page
1. An error message
1. An error flag
1. A list of US states used to populate the form on the Checkout page

### Checkout.cshtml 
This view is where the user can enter their purchase amount and location, and also where the tax rate, tax amount and new total with tax are displayed to the user.

### TaxServiceController.cs
The TaxServiceController consists of 5 methods:

#### Index
Just redirects to the Checkout page.

#### Checkout (Get)
Renders an empty form and populates an error message used by the client side validation Javascript.

#### Checkout (Post)
The Checkout POST methods does a couple different things:

1. Builds a TaxResultViewModel object using information entered into the form by the user and calling the GetTaxResultForOrder method. This object is then returned to the view and used to populate the tax rate, tax amount, and total with tax in the existing form. 

1. Checks for a bad request due to malformed data or state/zipcode mismatch and returns a relevant error message to the view.

1. After a tax calculation has completed successfully this method can reset the form to allow for another tax calculation.

#### GetTaxResultForOrder
Takes a TaxRequest and passes that object along with API credentials gathered from a config file to the TaxCalculator. Returns a TaxResult object.

#### GetTaxRateForLocation
Takes a TaxRequest and passes that object along with API credentials gathered from a config file to the TaxCalculator. Returns the tax rate as a decimal number.

### Test Project
TaxService.Tests contains one controller with 3 test methods:

1. Checkout - tests all 3 ViewResults of the Checkout methods
1. GetTaxRateForLocation - tests that a decimal number greater than 0 is returned when supplied with a valid address and purchase amount.
1. GetTaxResultForOrder - tests that a valid object of type TaxResult is returned when supplied with a valid address and purchase amount.

------------------------------------------------------------------

## Javascript
The TaxService website uses Bootstrap and jQuery, and a custom form validation function in core.js.

------------------------------------------------------------------

## CSS
The TaxService website uses Bootstrap and some additional custom CSS in Site.css
