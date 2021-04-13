using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class EmpViewModel
    {
        public List<Employee> EmpList { get; set; } = new List<Employee>();

        [Display(Name = "ユーザID")]
        public int? id_employee { get; set; }

        [Display(Name = "ユーザ名")]
        public string nm_employee { get; set; }

        [Display(Name = "ユーザ名ふりがな")]
        public string kn_employee { get; set; }

        [Display(Name = "メールアドレス")]
        public string mail_address { get; set; }

        [Display(Name = "パスワード")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "部署ID")]
        public int id_department { get; set; }

        [Display(Name = "管理者フラグ")]
        public string flg_admin { get; set; }

        [Display(Name = "退職フラグ")]
        public string flg_retirement { get; set; }

        public int id_update { get; set; }

        public DateTime date_update { get; set; }
    }
}
