using System.ComponentModel.DataAnnotations;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Entities.Account.Role;

namespace Eshop.Domain.Dtos.Account.User
{
    public class FilterUserDto : BasePaging
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string EmailActiveCode { get; set; }
        public bool IsEmailActive { get; set; }
        public string Mobile { get; set; }
        public string MobileActiveCode { get; set; }
        public bool IsMobileActive { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public bool IsBlocked { get; set; }
        public Role Role { get; set; }
        public List<Role> Roles { get; set; }
        public List<Entities.Account.User.User> Users { get; set; }
        public FilterUserRole UserRole { get; set; }

        public FilterUserDto SetUsers(List<Entities.Account.User.User> users)
        {
            this.Users = users;
            return this;
        }

        public FilterUserDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }


    }

    public enum FilterUserRole
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "مدیر سیستم")]
        SystemAdmin,

        [Display(Name = "کاربر سیستم")]
        SystemUser,

        [Display(Name = "محتوا گذار")]
        ContainUploader,
    }
}
