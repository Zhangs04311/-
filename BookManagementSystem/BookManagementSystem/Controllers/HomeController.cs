using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookManagementSystem.Models;
using BookManagementSystem.Daos;
using BookManagementSystem.Utils;
using BookManagementSystem.Extensions;

namespace BookManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpContext.Session.Remove("ACCOUNT_INFO");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");
            if(emp == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Account));
            }           
            
        }
        public IActionResult Account()
        {
            Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");

            return View(emp);
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                EmployeeDao dao = new EmployeeDao(DBUtil.GetConnection());
                var result = dao.SelectForLogin(model.mail_address, model.Password);
                if (result.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "入力したメールアドレスまたはパスワードが間違っています");
                    
                    return View(model);
                }
                else
                {
                    HttpContext.Session.SetObject<Employee>("ACCOUNT_INFO", result[0]);
                    Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");
                    if(int.Parse(emp.flg_admin) == 1)
                    {
                        return RedirectToAction("Index", "Manager");
                    }
                    else
                    {
                        return RedirectToAction("Index", "User");
                    }
                }

            }

            return View();
            
        }
        public IActionResult SelectBook(SelectBookModel model)
        {
            SelectBookModel selectModel = new SelectBookModel();          
            if (ModelState.IsValid)
            {
                BookDao dao = new BookDao(DBUtil.GetConnection());
                List<Book> result = null;

                if (!string.IsNullOrEmpty(model.SelectedName))
                {
                    result = dao.SelectByName(model.SelectedName);
                }
                else if (!string.IsNullOrEmpty(model.SelectedPub))
                {
                    result = dao.SelectByPub(model.SelectedPub);
                }
                else if (!string.IsNullOrEmpty(model.SelectNote))
                {
                    result = dao.SelectByNote(model.SelectNote);
                }
                else
                {
                    result = dao.SelectAll();
                }
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].flg_disposal == "1")
                    {
                        result.RemoveAt(i);
                    }             
                }
                foreach(var item in result)
                {
                    if (item.status == "1" || item.status == "3")
                    {
                        item.status = "貸出可能";
                    }
                    if (item.status == "0" || item.status == "2")
                    {
                        item.status = "貸出中";
                    }
                }
                selectModel.BookList = result;

            }
            return View(selectModel);
        }
        public IActionResult Menu()
        {
            Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");
            if(emp == null)
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                if (int.Parse(emp.flg_admin) == 1)
                {
                    return RedirectToAction("Index", "Manager");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }
            
        }
    }
    
    

    /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }*/

}
