using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Employee
    {

        [Display(Name = "ユーザID")]
        public int? id_employee { get; set; }

        [Display(Name = "ユーザ名")]
        [Required(ErrorMessage = "{0}は必須です")]
        public string nm_employee { get; set; }

        [Display(Name = "ユーザ名ふりがな")]
        [Required(ErrorMessage = "{0}は必須です")]
        public string kn_employee { get; set; }

        [Display(Name = "メールアドレス")]
        [Required(ErrorMessage = "{0}は必須です")]
        public string mail_address { get; set; }

        [Display(Name = "パスワード")]
        [Required(ErrorMessage = "{0}は必須です")]
        public string password { get; set; }

        [Display(Name = "部署ID")]
        [Range(3,10, ErrorMessage ="3から10までの数字を入力してください")]
        [Required(ErrorMessage = "{0}は必須です")]
        public int id_department { get; set; }
        
        [Display(Name ="管理者Flag")]
        [Required(ErrorMessage = "{0}は必須です")]
        public string flg_admin { get; set; }
        
        [Display(Name = "退職Flag")]
        [Required(ErrorMessage = "{0}は必須です")]
        public string flg_retirement { get; set; }
        
        public int id_update { get; set; }
        
        public DateTime date_update { get; set; }
    }
}
