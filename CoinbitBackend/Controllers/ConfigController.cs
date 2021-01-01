using CoinbitBackend.Entities;
using CoinbitBackend.Extension;
using CoinbitBackend.Models;
using CoinbitBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoinbitBackend.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        DBRepository _dBRepository;

        public ConfigController(DBRepository dBRepository)
        {
            _dBRepository = dBRepository;
        }

        [HttpPost("updateconfig")]
        public async Task<object> UpdateConfig([FromBody] Config config)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                
                if (_dBRepository.Config.Any())
                {
                    var update = await _dBRepository.Config.FirstOrDefaultAsync();
                    update.TetherRialValue = config.TetherRialValue;
                    await _dBRepository.SaveChangesAsync();
                }
                else
                {
                    await _dBRepository.Config.AddAsync(config);
                    await _dBRepository.SaveChangesAsync();
                }
                
                return new CoreResponse() { data = config, isSuccess = true };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = config, isSuccess = false };
            }
        }


        [HttpGet("get")]
        public async Task<object> GetConfig() 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var lst = await _dBRepository.Config.AsNoTracking().ToListAsync();
                return Ok(lst);

            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = null, isSuccess = false };
            }
        }
    }
}
