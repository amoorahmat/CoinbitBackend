using CoinbitBackend.Entities;
using CoinbitBackend.Extension;
using CoinbitBackend.Models;
using CoinbitBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

                var cususer = new User() { createDate = DateTime.Now, Password = string.Empty, UserName = mobile, UserRole = 2 };
                await _dBRepository.Users.AddAsync(cususer);
                await _dBRepository.SaveChangesAsync();

                var cus = new Customer() { firstName = result.firstName, lastName = result.lastName, mobile = result.mobile, email = result.mail, StatusId = 1, user_id = cususer.Id };
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

        [HttpPost("customerchangepwd")]        
        public async Task<object> CustomerChangePassword(string mobile, string password)    
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cus = await _dBRepository.Customers.FirstOrDefaultAsync(u => u.mobile == mobile);

                if (cus == null)
                    throw new Exception("customer is not exists");

                var user = await _dBRepository.Users.FirstOrDefaultAsync(a => a.Id == cus.user_id);

                if (user == null)
                    throw new Exception("customer's user is not exists");

                if (!string.IsNullOrWhiteSpace(user.Password))
                    throw new Exception("this customer has been set password before and not in sign up step.");

                user.Password = password;
                await _dBRepository.SaveChangesAsync();

                return new CoreResponse() { data = null, isSuccess = true,devMessage = "password changes successfully." };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = null, isSuccess = false };
            }
        }
    }
}
