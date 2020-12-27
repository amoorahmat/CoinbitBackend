using CoinbitBackend.Entities;
using CoinbitBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoinbitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinController : Controller
    {
        private DBRepository dBRepository;
        private CacheManager cacheManager;
        public CoinController(DBRepository dBRepository, CacheManager cacheManager)
        {
            this.dBRepository = dBRepository;
            this.cacheManager = cacheManager;
        }


        [HttpGet("get")]
        public async Task<ActionResult> GetCoinData()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cachedata = cacheManager.GetCoinLog();
                if (cachedata != null && cachedata.Count() > 0)
                    return Ok(cachedata);
                else
                {
                    var result = dBRepository.CoinDatas.AsNoTracking().OrderByDescending(u => u.SeriesDate).ThenBy(u => u.Ranking).Take(150).ConvertToCoinDataView();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

    }
}
