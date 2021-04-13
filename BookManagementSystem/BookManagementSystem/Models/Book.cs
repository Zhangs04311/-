using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Book
    {
        [Display(Name ="蔵書ID")]
        public int id_book { get; set; }
        
        [Display(Name = "ISBN")]
        [Required(ErrorMessage ="{0}は必須です")]
        public string isbn { get; set; }
        
        [Display(Name = "書籍名")]
        [Required(ErrorMessage = "{0}は必須です")]
        public string nm_book{get; set;}
        
        [Display(Name = "書籍名よみ")]
        public string kn_book { get; set; }
        
        [Display(Name = "出版社")]
        public string publisher { get; set; }
        
        [Display(Name = "更新者")]
        public int id_update { get; set; }
        
        [Display(Name = "更新日")]
        public DateTime date_update { get; set; }
        
        [Display(Name = "特記事項")]
        public string note { get; set; }
        
        [Display(Name = "廃棄Flag（0:貸出可　1:廃棄）")]
        [Required(ErrorMessage = "{0}は必須です")]
        public string flg_disposal { get; set; }
        public string status { get; set; }
    }
}
