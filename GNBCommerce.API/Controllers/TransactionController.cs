using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNBCommerce.Application.Models;
using GNBCommerce.Application.Services.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GNBCommerce.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        #region Dependencies
        readonly ITransactionService _transactionService;
        #endregion

        #region Constructor
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Transactions list
        /// </summary>
        /// <remarks>
        /// GET Transaction complete list
        /// </remarks>
        /// <returns>List TransactionDto</returns>
        [HttpGet("")]
        public IEnumerable<TransactionDto> GetAll()
        {
            return _transactionService.GetAll();
        }
        #endregion
    }
}