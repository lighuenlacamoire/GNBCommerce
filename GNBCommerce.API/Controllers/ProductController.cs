using GNBCommerce.Application.Models;
using GNBCommerce.Application.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GNBCommerce.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region Dependencies
        readonly IProductService _productService;
        #endregion

        #region Constructor
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        [HttpGet("{sku}")]
        public ProductDto GetBySKU(string sku)
        {
            return _productService.GetBySKU(sku);
        }

        [HttpGet("{sku}/Rate/{currency}")]
        public ProductDto GetBySKUAndCurrency(string sku, string currency)
        {
            return _productService.GetBySKU(sku, currency);
        }
    }
}