using Eshop.Application.Services.Implementation;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Repository;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Implementations;
using MarketPlace.Application.Services.Interfaces;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Eshop.Application.Services.Implementations;
using Eshop.Application.Utilities;

namespace ServiceHost.Configuration
{
    public static class DiContainer
    {

        public static void RegisterService(this IServiceCollection services)
        {


            #region Repository

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #endregion

            #region General Services

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISiteSettingService, SiteSettingService>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<ISiteImagesService, SiteImagesService>();
            
           

            #endregion

            #region Common Services

            services.AddHttpContextAccessor();
            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
           services.AddTransient<IAuthHelper, AuthHelper>();

            #endregion

        }

    }
}
