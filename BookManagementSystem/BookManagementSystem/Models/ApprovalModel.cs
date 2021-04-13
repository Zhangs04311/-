using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Models
{
    public class ApprovalModel
    {
        public List<Borrow> borrowList { get; set; } = new List<Borrow>();
    }
}
