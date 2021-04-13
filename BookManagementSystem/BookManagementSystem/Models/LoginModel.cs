using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class LoginModel
    {
        [Display(Name = "メールアドレス")]
        [Required(ErrorMessage = "{0}を入力してください")]
        public string mail_address { get; set; }

        [Display(Name = "パスワード")]
        [Required(ErrorMessage = "{0}を入力してください")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Flg_admin { get; set; }
    }
}
