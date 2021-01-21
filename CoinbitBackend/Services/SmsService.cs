using CoinbitBackend.Entities;
using CoinbitBackend.Models;
using Kavenegar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinbitBackend.Services
{
    public class SmsService
    {
        private static List<CustomerRegisterReqSms> CustomerRegList;
        private static KavenegarApi kavenegarApi;

        public SmsService(string apikey)
        {
            CustomerRegList = new List<CustomerRegisterReqSms>();
            kavenegarApi = new KavenegarApi(apikey);
        }

        public async Task SendSms(CustomerRegisterReq customer) 
        {
            RemoveExpiredFromList();

            if (CustomerRegList.Any(l => l.mobile == customer.mobile))
            {
                throw new Exception("شماره شما در سیستم ثبت شده است، کد ارسال شده توسط پیامک را وارد کنید یا چند دقیقه صبر کنید تا منقضی شود و دوباره تلاش کنید");
            }

            var obj = new CustomerRegisterReqSms()
            {
                firstName = customer.firstName,
                lastName = customer.lastName,
                mail = customer.mail,
                mobile = customer.mobile,
                createDate = DateTime.Now,
                SmsCode = RandomString(5)
            };

            var result = await kavenegarApi.Send(string.Empty, obj.mobile, obj.SmsCode);
            obj.SmsSendResult = result;
            CustomerRegList.Add(obj);

            if (result.Messageid <= 0)
                throw new Exception(result.StatusText);            
        }

        public CustomerRegisterReqSms CheckSmsCode(string mobile,string code)
        {
            RemoveExpiredFromList();

            var obj = CustomerRegList.FirstOrDefault(p=>p.mobile == mobile);

            if (obj == null)
                throw new Exception("موبایل وارد شده معتبر نیست و در مرحله ثبت اولیه وارد نشده است که پیامک برایش ارسال شود");

            if (obj.SmsCode != code)
                throw new Exception("کد وارد شده معتبر نمی باشد");

            return obj;
        }

        public void RemoveCusFromList(string mobile)  
        {
            var obj = CustomerRegList.FirstOrDefault(p => p.mobile == mobile);

            if (obj != null)
                CustomerRegList.Remove(obj);
        }

        private void RemoveExpiredFromList()   
        {
            var tmp = CustomerRegList.Where(a => (DateTime.Now - a.createDate).TotalSeconds < 180);
            CustomerRegList = new List<CustomerRegisterReqSms>();
            CustomerRegList.AddRange(tmp);
        }

        private static string RandomString(int length)
        {
            var random = new Random();
            const string pool = "0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}
