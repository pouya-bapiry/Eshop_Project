using Eshop.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application.Services.Implementation
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendRecoverPasswordSms(string mobile, string newPassword)
        {
            var apiKey = _configuration.GetSection("KavenegarSmsApiKey")["apiKey"];

            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
            await api.VerifyLookup(mobile, newPassword, "VerifyRecoverPassword");
        }

        public async Task SendVerificationSms(string mobile, string activationCode)
        {
            var apiKey = _configuration.GetSection("KavenegarSmsApiKey")["apiKey"];

            Kavenegar.KavenegarApi api=new Kavenegar.KavenegarApi(apiKey);
            await api.VerifyLookup( mobile, activationCode, "VerifyWebsiteAccount");
        }
    }
}
