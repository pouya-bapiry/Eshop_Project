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

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<ISiteImagesService, SiteImagesService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductDiscountService, ProductDiscountService>();




            #endregion

            #region Common Services

            services.AddHttpContextAccessor();
            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
           services.AddScoped<IAuthHelper, AuthHelper>();

            #endregion

        }

    }
}
