using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class SelectEmpModel
    {
        public List<Employee> EmpList { get; set; } = new List<Employee>();
        
        [Display(Name ="ユーザ名")]       
        public string SelectedName { get; set; }

        [Display (Name ="メールアドレス")]
        [EmailAddress(ErrorMessage ="入力したメールアドレスの形式は正しくありません")]
        public string SelectedMail { get; set; }

        [Display(Name = "部署ID")]
        [Range(3, 10, ErrorMessage = "sasa")]
        public int? SelectedDepId { get; set; }
    }
}
