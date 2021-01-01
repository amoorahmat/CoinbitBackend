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
    public class FavCoinsController : Controller
    {
        DBRepository _dBRepository;

        public FavCoinsController(DBRepository dBRepository)
        {
            _dBRepository = dBRepository;
        }

        [HttpGet("getall")]
        public async Task<object> GetFavCoins()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var lst = await _dBRepository.FavCoins.AsNoTracking().ToListAsync();
                return Ok(lst);
                
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = null, isSuccess = false };
            }
        }


        [HttpPost("addfavcoin")]
        public async Task<object> AddFavCoin(string CoinId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (_dBRepository.FavCoins.Any(l => l.CoinId == CoinId))
                    throw new Exception("this coin already exists in fav list, babe!");

                var obj = new Entities.FavCoins() { CoinId = CoinId };
                await _dBRepository.FavCoins.AddAsync(obj);
                await _dBRepository.SaveChangesAsync();

                return new CoreResponse() { data = obj, isSuccess = true };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = new FavCoins() { CoinId = CoinId }, isSuccess = false };
            }
        }


        [HttpPost("removefavcoin")]
        public async Task<object> RemoveFavCoin(string CoinId)   
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (!_dBRepository.FavCoins.Any(l => l.CoinId == CoinId))
                    throw new Exception($"this is no fav coin in the list with this CoinId: {CoinId}, babe!");

                var obj = _dBRepository.FavCoins.FirstOrDefault(l=>l.CoinId == CoinId);
                if(obj != null)
                {
                    _dBRepository.FavCoins.Remove(obj);
                    await _dBRepository.SaveChangesAsync();
                }
                
                return new CoreResponse() { data = obj, isSuccess = true };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = new FavCoins() { CoinId = CoinId }, isSuccess = false };
            }
        }
    }
}
