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
        SmsService _smsService;

        public CustomerController(DBRepository dBRepository,SmsService smsService)
        {
            _dBRepository = dBRepository;
            _smsService = smsService;
        }

        [HttpPost("registerCustomer")]
        public async Task<object> AddCustomer([FromBody] CustomerRegisterReq request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                //var cus = new Customer() { firstName = request.firstName, lastName = request.lastName, mobile = request.mobile, email = request.mail };
                //await _dBRepository.Customers.AddAsync(cus);
                //await _dBRepository.SaveChangesAsync();

                await _smsService.SendSms(request);

                return new CoreResponse() { data = request, isSuccess = true };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = request, isSuccess = false };
            }
        }


        [HttpPost("checkcustomersms")]
        public async Task<object> CheckCustomerSms(string mobile,string code)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _smsService.CheckSmsCode(mobile, code);

                var cus = new Customer() { firstName = result.firstName, lastName = result.lastName, mobile = result.mobile, email = result.mail, StatusId = 1 };
                await _dBRepository.Customers.AddAsync(cus);
                await _dBRepository.SaveChangesAsync();

                _smsService.RemoveCusFromList(mobile);

                return new CoreResponse() { data = cus, isSuccess = true };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = null, isSuccess = false };
            }
        }
    }
}
