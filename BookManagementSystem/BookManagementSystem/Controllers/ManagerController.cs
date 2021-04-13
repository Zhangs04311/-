using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookManagementSystem.Models;
using BookManagementSystem.Daos;
using BookManagementSystem.Utils;
using BookManagementSystem.Extensions;

namespace BookManagementSystem.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SelectEmp(SelectEmpModel model)
        {
            SelectEmpModel selectModel = new SelectEmpModel();
            if (ModelState.IsValid)
            {
                EmployeeDao dao = new EmployeeDao(DBUtil.GetConnection());
                List<Employee> result = null;
                if(!string.IsNullOrEmpty(model.SelectedName))
                {                  
                    result = dao.SelectByName(model.SelectedName);
                }
                else if(!string.IsNullOrEmpty(model.SelectedMail))
                {
                    result = dao.SelectByMail(model.SelectedMail);
                }
                else if(model.SelectedDepId != 0 && model.SelectedDepId != null)
                {
                    var id = model.SelectedDepId.Value;
                    result = dao.SelectByDepId(id);                  
                }
                else
                {
                    result = dao.SelectAll();
                }
                for(var i = 0; i < result.Count; i ++)
                {
                    if (result[i].flg_retirement == "1")
                    {
                        result.RemoveAt(i);
                    }
                }
                selectModel.EmpList = result;
            }
            
            return View(selectModel);
        }

        [HttpGet("Manager/UpdateEmp/{id?}")]
        public IActionResult UpdateEmp(int id)
        {
            HttpContext.Session.SetInt32("id_emp", id);
            EmployeeDao dao = new EmployeeDao(DBUtil.GetConnection());
            List<Employee> result = dao.SelectById(id);
            Employee model = result[0];

            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateEmp(Employee model)
        {
            if (ModelState.IsValid)
            {
                EmployeeDao dao = new EmployeeDao(DBUtil.GetConnection());
                Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");
                model.id_update = emp.id_employee.Value;
                int id = HttpContext.Session.GetInt32("id_emp").Value;
                
                if(dao.UpdatetEmployee(id, model) > 0)
                {
                    return Content("<head><meta charset='UTF-8'></head><script>alert('社員情報が編集されました！');history.go(-2);location.reload();</script>", "text/html");
                }
                else
                {
                    return Content("<script>alert('Failed!');history.go(-1);location.reload();</script>", "text/html");
                }
                

            }
            return RedirectToAction(nameof(Index));


        }

        [HttpGet]
        public IActionResult InsertEmp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InsertEmp(Employee model)
        {
            if (ModelState.IsValid)
            {
                EmployeeDao dao = new EmployeeDao(DBUtil.GetConnection());
                Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");
                model.id_update = emp.id_employee.Value;
                if(dao.InsertEmployee(model) > 0)
                {
                    return Content("<head><meta charset='UTF-8'></head><script>alert('新規社員が登録されました!');history.go(-2);location.reload();</script >", "text/html");
                }
                else
                {
                    return Content("<script>alert('Failed!');history.go(-1);location.reload();</script >", "text/html");
                }
                
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult InsertBook()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InsertBook(Book model)
        {
            if (ModelState.IsValid)
            {
                BookDao dao = new BookDao(DBUtil.GetConnection());
                Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");
                model.id_update = emp.id_employee.Value;
                if(model.kn_book == null)
                {
                    model.kn_book = "";
                }
                if(model.publisher == null)
                {
                    model.publisher = "";
                }
                if(model.note == null)
                {
                    model.note = "";
                }
               
                if(dao.InsertBook(model) > 0)
                {
                    return Content("<head><meta charset='UTF-8'></head><script>alert('書籍が登録されました！');history.go(-2);location.reload();</script >", "text/html");
                }
                else
                {
                    return Content("<script>alert('Failed!');history.go(-1);location.reload();</script >", "text/html");
                }
                
            }
            else
            {
                return View();
            }
        }
        public IActionResult SelectBook()
        {
            return View();
        }

        
        public IActionResult UpdateBook(SelectBookModel model)
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
                selectModel.BookList = result;

            }
            return View(selectModel);
        }
        [HttpGet("Manager/UpdateBook2/{id?}")]
        public IActionResult UpdateBook2(int id)
        {
            HttpContext.Session.SetInt32("id_book", id);
            BookDao dao = new BookDao(DBUtil.GetConnection());
            List<Book> result = dao.SelectById(id);
            Book model = result[0];

            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateBook2(Book model)
        {
            if (ModelState.IsValid)
            {
                BookDao dao = new BookDao(DBUtil.GetConnection());
                int id = HttpContext.Session.GetInt32("id_book").Value;
                Employee emp = HttpContext.Session.GetObject<Employee>("ACCOUNT_INFO");
                model.id_update = emp.id_employee.Value;
                if(dao.UpdateBook(id, model) > 0)
                {
                    return Content("<head><meta charset='UTF-8'></head><script>alert('書籍が編集されました！');history.go(-2);location.reload();</script >", "text/html");
                }
                else
                {
                    return Content("<script>alert('Failed');history.go(-2);location.reload();</script >", "text/html");
                }                              
            }
            return RedirectToAction(nameof(UpdateBook));
        }
        public IActionResult Approval()
        {
            ApprovalModel model = new ApprovalModel();
            BorrowDao dao = new BorrowDao(DBUtil.GetConnection());
            var list = new List<Borrow>();
            var result = dao.SelectForApproval();
            
            foreach(var item in result)
            {
                if(item.status == "0")
                {
                    list.Add(item);
                }
            }

            model.borrowList = list;

            return View(model);
        }
        [HttpPost]
        public ActionResult Approval(string[] checks)
        {
            if(checks.Length == 0)
            {
                return Content("<head><meta charset='UTF-8'></head><script >alert('承認項目を選択してください');</script >", "text/html");
            }
            else
            {
                List<int> id = new List<int>();
                foreach (var item in checks)
                {
                    id.Add(int.Parse(item));
                }

                BorrowDao dao = new BorrowDao(DBUtil.GetConnection());
                int result = dao.UpdateForApproval(id);
                if(result > 0)
                {
                    return Content("<head><meta charset='UTF-8'></head><script>alert('承認が完了しました！');history.go(-1);location.reload();</script >", "text/html");
                }
                else
                {
                    return Content("<script>alert('Failed!');history.go(-2);location.reload();</script >", "text/html");
                }
            }
            

        }
    }
}
