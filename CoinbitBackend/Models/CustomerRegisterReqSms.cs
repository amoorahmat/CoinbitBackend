using Kavenegar.Core.Models;
using System;

namespace CoinbitBackend.Models
{
    public class CustomerRegisterReqSms : CustomerRegisterReq
    {
        public DateTime createDate { get; set; }
        public string SmsCode { get; set; }
        public SendResult SmsSendResult { get; set; }   

    }
}
