using CoinbitBackend.Models;
using CoinbitBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CoinbitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankController : Controller
    {
        DBRepository _dBRepository;

        public BankController(DBRepository dBRepository, SmsService smsService)
        {
            _dBRepository = dBRepository;
        }   

        [HttpGet("get_banks")]
        public async Task<ActionResult> GetBanks()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cus = await _dBRepository.banks.AsNoTracking().ToListAsync();

                return Ok(new CoreResponse() { isSuccess = true, data = cus });

            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }
    }
}
