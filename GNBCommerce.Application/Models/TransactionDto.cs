using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Application.Models
{
    public class TransactionDto
    {
        public string SKU { get; set; }

        public string Amount { get; set; }

        public string Currency { get; set; }

    }
}
