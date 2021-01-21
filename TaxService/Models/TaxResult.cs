using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxService.Models
{
    public class TaxResult
    {
        public Decimal TaxRate { get; set; }
        public Decimal TaxAmount { get; set; }
    }
}