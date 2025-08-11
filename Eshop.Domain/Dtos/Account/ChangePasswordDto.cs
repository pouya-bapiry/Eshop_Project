using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.Account
{
    public class ChangePasswordDto
    {
        [Display(Name ="رمز عبور فعلی")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(255,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string CurrentPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]

        public string NewPassword { get; set; }

        [Display(Name = " تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Compare("NewPassword", ErrorMessage = "کلمه های عبور مغایرت دارند")]

        public string ConfirmNewPassword { get; set; }


    }
    public enum ChangePasswordResult
    {
        Success,
        Error,
        WrongCurrentPassword,
        NewPasswordSameAsOld

    }

}
