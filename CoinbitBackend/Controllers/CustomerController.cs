using AutoMapper;
using CoinbitBackend.Entities;
using CoinbitBackend.Extension;
using CoinbitBackend.Models;
using CoinbitBackend.Models.pagination_reports;
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
        private DBDapperRepository dBDapperRepository;

        public CustomerController(DBRepository dBRepository, SmsService smsService, DBDapperRepository dBDapperRepository)
        {
            _dBRepository = dBRepository;
            _smsService = smsService;
            this.dBDapperRepository = dBDapperRepository;
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
                var cus = await _dBRepository.Customers.AnyAsync(a => a.mobile == request.mobile);
                if (cus)
                {
                    throw new Exception("there is already an exsiting customer with this mobile: " + request.mobile);
                }
                await _smsService.SendSms(request);
                return new CoreResponse() { data = request, isSuccess = true };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = request, isSuccess = false };
            }
        }


        [HttpPost("checkcustomersms")]
        public async Task<object> CheckCustomerSms(string mobile, string code)
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

                return new CoreResponse() { data = null, isSuccess = true, devMessage = "password changes successfully." };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = null, isSuccess = false };
            }
        }


        [HttpDelete("customerdelete_temporary")]
        public async Task<object> CustomerDelete_Temporary(string mobile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cus = await _dBRepository.Customers.FirstOrDefaultAsync(a => a.mobile == mobile);
                var user = await _dBRepository.Users.FirstOrDefaultAsync(a => a.UserName == mobile);

                _dBRepository.Users.Remove(user);
                _dBRepository.Customers.Remove(cus);

                await _dBRepository.SaveChangesAsync();

                return new CoreResponse() { data = null, isSuccess = true, devMessage = "customer deletes successfully." };
            }
            catch (Exception ex)
            {
                return new CoreResponse() { devMessage = ex.GetaAllMessages(), data = null, isSuccess = false };
            }
        }


        [HttpPost("updateCustomer")]
        [Authorize]
        public async Task<ActionResult> UpdateCustomer([FromBody] CustomerUpdateModel customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cus = await _dBRepository.Customers.Where(l => l.Id == customer.customer_id).FirstOrDefaultAsync();
                if (cus == null)
                {
                    throw new Exception("there is no customer with this id that passed in.");
                }

                cus.fatherName = customer.father_name;
                cus.nationalCode = customer.national_code;
                cus.birthDate = customer.birth_date;
                cus.bank_id = customer.bank_id;
                cus.card_number = customer.card_number;
                cus.tel = customer.tel;

                await _dBRepository.SaveChangesAsync();

                return Ok(new CoreResponse() { isSuccess = true, data = cus });

            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }

        [HttpPost("customer_set_idcardpic")]
        [Authorize]
        public async Task<ActionResult> CustomerSetIDCardPic(long cusid,string idcardpicname)   
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cus = await _dBRepository.Customers.Where(l => l.Id == cusid).FirstOrDefaultAsync();
                if (cus == null)
                {
                    throw new Exception("there is no customer with this id that passed in.");
                }

                cus.idcardpic = idcardpicname;

                await _dBRepository.SaveChangesAsync();

                return Ok(new CoreResponse() { isSuccess = true, data = cus });

            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }

        [HttpPost("customer_set_bankcardpic")]
        [Authorize]
        public async Task<ActionResult> CustomerSetBankCardPic(long cusid, string bankcardpic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cus = await _dBRepository.Customers.Where(l => l.Id == cusid).FirstOrDefaultAsync();
                if (cus == null)
                {
                    throw new Exception("there is no customer with this id that passed in.");
                }

                cus.bankcardpic = bankcardpic;

                await _dBRepository.SaveChangesAsync();

                return Ok(new CoreResponse() { isSuccess = true, data = cus });

            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }

        [HttpPost("customer_set_selfiepic")]
        [Authorize]
        public async Task<ActionResult> CustomerSetSelfiePic(long cusid, string selfiepic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var cus = await _dBRepository.Customers.Where(l => l.Id == cusid).FirstOrDefaultAsync();
                if (cus == null)
                {
                    throw new Exception("there is no customer with this id that passed in.");
                }

                cus.selfiepic = selfiepic;

                await _dBRepository.SaveChangesAsync();

                return Ok(new CoreResponse() { isSuccess = true, data = cus });

            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }

        [HttpGet("get")]
        [Authorize(Roles ="customer")]
        public async Task<ActionResult> GetCustomer()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var user_id = _dBRepository.Users.AsNoTracking().FirstOrDefault(a => a.UserName.ToLower() == User.Identity.Name.ToLower())?.Id;
                var cus = await _dBRepository.Customers.AsNoTracking().Where(l => l.user_id == user_id).FirstOrDefaultAsync();
                if (cus == null)
                {
                    throw new Exception("there is no customer with this id that passed in.");
                }

                return Ok(new CoreResponse() { isSuccess = true, data = cus });
            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }

        [HttpGet("getforadmin")]
        [Authorize(Roles = "admin,acc")]
        public async Task<ActionResult> GetCustomer(long customer_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                
                var cus = await _dBRepository.Customers.AsNoTracking().Where(l => l.user_id == customer_id).FirstOrDefaultAsync();
                if (cus == null)
                {
                    throw new Exception("there is no customer with this id that passed in.");
                }

                return Ok(new CoreResponse() { isSuccess = true, data = cus });
            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }

        [HttpGet("getallpaginate")]
        [Authorize(Roles = "admin,acc")]
        public async Task<ActionResult> GetAllCustomersPaginate(string first_name,string last_name,string mobile, int page = 1, int pagesize = 10)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var query = $" select count(1) OVER() AS row_count,* from public.\"Customers\" where 1 = 1 and \"firstName\" like '%{first_name}%' and \"lastName\" like '%{last_name}%' and mobile like '%{mobile}%' ORDER BY \"Id\"  LIMIT {pagesize}  OFFSET ({pagesize} * ({page}-1)) ";

                var cus = await dBDapperRepository.RunQueryAsync<CustomerReportModel>(query);

                //var cus = await _dBRepository.Customers.AsNoTracking().ToListAsync();

                return Ok(new CoreResponse() { isSuccess = true, data = cus, total_items = cus.First()?.row_count, current_page = page, total_pages = (cus.First()?.row_count / pagesize) + 1 });
            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }

        [HttpGet("getall")]
        [Authorize(Roles = "admin,acc")]
        public async Task<ActionResult> GetAllCustomers()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                

                var cus = await _dBRepository.Customers.AsNoTracking().ToListAsync();


                return Ok(new CoreResponse() { isSuccess = true, data = cus });
            }
            catch (Exception ex)
            {
                return Ok(new CoreResponse() { isSuccess = false, data = null, devMessage = ex.Message });
            }
        }
    }
}