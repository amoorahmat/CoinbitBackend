using CBCurrenciesFetcher;
using CoinbitBackend.Extension;
using CoinbitBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinbitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        CurrencyFetcher _currencyFetcher;

        public CurrencyController(CurrencyFetcher currencyFetcher)
        {
            _currencyFetcher = currencyFetcher;
        }



        [HttpGet("gold")]
        public async Task<ActionResult> GetGoldData()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                var result = await _currencyFetcher.GetGoldData();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

        [HttpGet("currency")]
        public async Task<ActionResult> GetCurrencyData()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                var result = await _currencyFetcher.GetCurrencyData();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
    }
}
