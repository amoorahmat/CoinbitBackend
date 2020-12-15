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
        private DBRepository _dBRepository;
        public CoinController(DBRepository dBRepository)
        {
            _dBRepository = dBRepository;
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

                var result = _dBRepository.CoinDatas.AsNoTracking().OrderByDescending(u => u.SeriesDate).ThenBy(u => u.Ranking).Take(150);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

    }
}
