using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BookManagementSystem.Models
{
    public class SelectBookModel
    {
        public List<Book> BookList { get; set; } = new List<Book>();
        
        [Display(Name ="書籍名")]
        public string SelectedName { get; set; }

        [Display(Name = "出版社")]
        public string SelectedPub { get; set; }

        [Display(Name = "特記事項")]
        public string SelectNote { get; set; }
    }
}
