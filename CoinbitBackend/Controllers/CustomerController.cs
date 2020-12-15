using CoinbitBackend.Entities;
using CoinbitBackend.Extension;
using CoinbitBackend.Models;
using CoinbitBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinbitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        DBRepository _dBRepository;

        public CustomerController(DBRepository dBRepository)
        {
            _dBRepository = dBRepository;
        }

        [HttpPost("registerCustomer")]
        public ActionResult<CoreResponse> AddCustomer([FromBody] CustomerRegisterReq request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cus = new Customer() { firstName = request.firstName, lastName = request.lastName, mobile = request.mobile, email = request.mail };
                _dBRepository.Customers.Add(cus);
                _dBRepository.SaveChanges();

                return new CoreResponse() { data = request, isSuccess = true };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = request, isSuccess = false };
            }
        }
    }
}
