using GNBCommerce.Application.Models;
using GNBCommerce.Application.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GNBCommerce.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        #region Dependencies
        readonly IRateService _rateService;
        #endregion

        #region Constructor
        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Rates list
        /// </summary>
        /// <remarks>
        /// GET Rates complete list
        /// </remarks>
        /// <returns>List RateDto</returns>
        [HttpGet("")]
        public IEnumerable<RateDto> GetAll()
        {
            return _rateService.GetAll();
        }

        // 
        //IEnumerable<IEnumerable<string>> result = GetPermutations(currencies, 2);
        //
        //static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        //{
        //    if (length == 1) return list.Select(t => new T[] { t });

        //    return GetPermutations(list, length - 1)
        //        .SelectMany(t => list.Where(e => !t.Contains(e)),
        //            (t1, t2) => t1.Concat(new T[] { t2 }));
        //}
        
        #endregion
    }
}