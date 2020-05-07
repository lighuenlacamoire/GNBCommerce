using GNBCommerce.Application.Models;
using GNBCommerce.Application.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Application.Services.Implementation
{
    public interface IProductService : IService<ProductDto>
    {
        ProductDto GetBySKU(string sku, string currency = "EUR");
    }
}
