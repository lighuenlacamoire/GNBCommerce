using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Application.Models
{
    public class RateDto
    {
        public string CurrencyFrom { get; set; }

        public string CurrencyTo { get; set; }

        public string Conversion { get; set; }
    }
}
