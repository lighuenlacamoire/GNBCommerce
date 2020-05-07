using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Application.Models
{
    public class ProductDto
    {
        public ProductDto()
        {
            Transactions = new List<TransactionDto>();
        }

        public string SKU { get; set; }
        public string TotalAmount { get; set; }
        public string Currency { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}
