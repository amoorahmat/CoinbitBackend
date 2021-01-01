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
        private DBDapperRepository dBDapperRepository;
        private CacheManager cacheManager;
        public CoinController(DBRepository dBRepository, CacheManager cacheManager,DBDapperRepository dBDapperRepository)
        {
            this.dBRepository = dBRepository;
            this.cacheManager = cacheManager;
            this.dBDapperRepository = dBDapperRepository;
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
                
                var config = await dBRepository.Config.AsNoTracking().FirstOrDefaultAsync();

                var cachedata = cacheManager.GetCoinLog();
                if (cachedata != null && cachedata.Count() > 0)
                    return Ok(cachedata.ConvertToCoinDataView(config.TetherRialValue));
                else
                {
                    var lst = await dBDapperRepository.RunQueryAsync<CoinData>(" WITH lastseriesdate AS (   SELECT \"SeriesDate\"    FROM public.\"CoinDatas\"   order by \"Id\" desc	limit 1) SELECT * FROM public.\"CoinDatas\" WHERE \"SeriesDate\" = (SELECT \"SeriesDate\" FROM lastseriesdate) ");                    
                    return Ok(lst.ConvertToCoinDataView(config.TetherRialValue));
                }
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }


        [HttpGet("getfav")]
        public async Task<ActionResult> GetCoinDataFav()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var config = await dBRepository.Config.AsNoTracking().FirstOrDefaultAsync();

                var cachedata = cacheManager.GetCoinLog();
                if (cachedata != null && cachedata.Count() > 0)
                    return Ok(cachedata.Where(p => p.IsFav).ToList().ConvertToCoinDataView(config.TetherRialValue));
                else
                {
                    //var lst = dBDapperRepository.RunQuery<CoinData>(" WITH lastseriesdate AS ( SELECT \"SeriesDate\" FROM public.\"CoinDatas\" order by \"Id\" desc limit 1 ) SELECT * FROM public.\"CoinDatas\" WHERE \"SeriesDate\" = (SELECT \"SeriesDate\" FROM lastseriesdate) AND  \"IsFav\" = true; ");
                    var lst = await dBDapperRepository.RunQueryAsync<CoinData>(" WITH lastseriesdate AS ( SELECT \"SeriesDate\" FROM public.\"CoinDatas\" order by \"Id\" desc limit 1 ) SELECT * FROM public.\"CoinDatas\" WHERE \"SeriesDate\" = (SELECT \"SeriesDate\" FROM lastseriesdate) AND  \"IsFav\" = true; ");
                    if (lst != null)
                    {
                        var result = lst.ConvertToCoinDataView(config.TetherRialValue);
                        return Ok(result);
                    }

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
    }
}