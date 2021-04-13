using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Models
{
    public class Borrow
    {
        public List<Book> BookList { get; set; } = new List<Book>();
        public int id_request { get; set; }
        public int id_employee { get; set; }
        public int id_book { get; set; }
        public DateTime date_request { get; set; }
        public string status { get; set; }
        public int? id_approval { get; set; }
        public DateTime? date_approval { get; set; }
        public DateTime? date_borrow { get; set; }
        public DateTime? date { get; set; }
        public DateTime? date_return { get; set; }
        public int id_update { get; set; }
        public DateTime date_update { get; set; }
    }
}
