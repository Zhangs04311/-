using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookManagementSystem.Daos;
using BookManagementSystem.Utils;
using BookManagementSystem.Models;
using BookManagementSystem.Extensions;


namespace BookManagementSystem.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("User/Apply/{id?}")]
        public IActionResult Apply(int id)
        {
            HttpContext.Session.SetInt32("id_book", id);
            BookDao dao = new BookDao(DBUtil.GetConnection());
            Borrow model = new Borrow();
            List<Book> result = null;

            result = dao.SelectById(id);
            model.BookList = result;

            return View(model);
        }
        [HttpPost]
        public IActionResult Apply(Borrow model)
        {
            if(model.date_borrow == null)
            {
                return Content("<head><meta charset = 'UTF-8' ></head ><script> alert('貸出日を選択してください！');history.go(-1);location.reload();</script> ", "text / html");

            }
            else if(model.date == null)
            {
                return Content("<head><meta charset = 'UTF-8' ></head ><script> alert('返却日を選択してください！');history.go(-1);location.reload();</script> ", "text / html");

            }
            else
            {
                BorrowDao dao = new BorrowDao(DBUtil.GetConnection());
                model.id_book = HttpContext.Session.GetInt32("id_book").Value;
                Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");
                model.id_employee = emp.id_employee.Value;
                dao.InsertBorrow(model);

                return RedirectToAction(nameof(Apply2));
            }
            
        }
        public IActionResult Apply2()
        {
            return View();
        }
    }
}
