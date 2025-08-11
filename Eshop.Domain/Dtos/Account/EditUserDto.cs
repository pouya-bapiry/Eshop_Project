using System.ComponentModel.DataAnnotations;
using Eshop.Domain.Entities.Account.Role;

namespace Eshop.Domain.Dtos.Account.User
{
    public class EditUserDto
    {
        #region Properties

        public long Id { get; set; }
        public long RoleId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string RoleName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }

        [Display(Name = "بلاک شده / نشده")]
        public bool IsBlocked { get; set; }

        [Display(Name = "موبایل فعال شده / نشده")]
        public bool IsMobileActivated { get; set; }

        public string Image { get; set; }

        public List<Role> Roles { get; set; }

        public List<long> RoleSelectedCategories { get; set; }



        #endregion
    }

    public enum EditUserResult
    {
        Success,
        UserNotFound,
        Error
    }
}
