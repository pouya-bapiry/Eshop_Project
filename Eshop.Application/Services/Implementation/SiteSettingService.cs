using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Site;
using Eshop.Domain.Entities.Site;
using Eshop.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application.Services.Implementation
{
    public class SiteSettingService : ISiteSettingService
    {

        #region Fields

        private readonly IGenericRepository<SiteSetting> _siteSettingRepository;

        private readonly IGenericRepository<AboutUs> _aboutUsRepository;

        #endregion


        #region Constructor

        public SiteSettingService(IGenericRepository<SiteSetting> siteSettingRepository, IGenericRepository<AboutUs> aboutUsRepository)
        {
            _siteSettingRepository = siteSettingRepository;
            _aboutUsRepository = aboutUsRepository;
        }


        #endregion


        #region Site Setting

        public async Task<SiteSettingDto> GetDefaultSiteSetting()
        {


            var siteSetting = await _siteSettingRepository.GetQuery().AsQueryable()
                 .Select(x => new SiteSettingDto
                 {
                     SiteName = x.SiteName,
                     Email = x.Email,
                     Address = x.Address,
                     CopyRight = x.CopyRight,
                     FooterText = x.FooterText,
                     IsDefault = x.IsDefault,
                     MapScript = x.MapScript,
                     Mobile = x.Mobile,
                     Phone = x.Phone,
                     CreateDate = x.CreateDate.ToStringShamsiDate(),
                     LastUpdateDate = x.LastUpdateDate.ToStringShamsiDate()
                 })
                 .FirstOrDefaultAsync(x => x.IsDefault);

            return siteSetting ?? new SiteSettingDto();
        }

        #endregion

        #region AboutUs
        public async Task<List<AboutUsDto>> GetAboutUs()
        {
            return await _aboutUsRepository.GetQuery().AsQueryable()
                .Select(x => new AboutUsDto
                {
                    HeaderTitle = x.HeaderTitle,
                    Description = x.Description

                }).ToListAsync();
        }

        #endregion

        #region Edit Site Setting

        public async Task<EditSiteSettingDto> GetSiteSettingForEdit(long id)
        {
            var setting = await _siteSettingRepository
                  .GetQuery()
                  .AsQueryable()
                  .SingleOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return null;
            }

            return new EditSiteSettingDto
            {
                Id = setting.Id,
                Address = setting.Address,
                CopyRight = setting.CopyRight,
                Email = setting.Email,
                FooterText = setting.FooterText,
                IsDefault = setting.IsDefault,
                MapScript = setting.MapScript,
                Mobile = setting.Mobile,
                Phone = setting.Phone
            };
        }

        public async Task<bool> EditSiteSetting(EditSiteSettingDto edit, string username)
        {
            var mainSetting = await _siteSettingRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == edit.Id);
            if (mainSetting == null)
            {
                return false;
            }
            mainSetting.Id = edit.Id;
            mainSetting.Address = edit.Address;
            mainSetting.CopyRight = edit.CopyRight;
            mainSetting.Email = edit.Email;
            mainSetting.FooterText = edit.FooterText;
            mainSetting.MapScript = edit.MapScript;
            mainSetting.Mobile = edit.Mobile;
            mainSetting.Phone = edit.Phone;

            _siteSettingRepository.EditEntityByUser(mainSetting,username);
            _siteSettingRepository.SaveChanges();
            return true;

        }


        #endregion




        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_siteSettingRepository != null)
            {
                await _siteSettingRepository.DisposeAsync();
            }
        }





        #endregion
    }
}