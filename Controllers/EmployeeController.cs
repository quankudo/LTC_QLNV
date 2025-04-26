using ProjectASP01.Models;
using ProjectASP01.Models.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ProjectASP01.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly companydbEntities _context;
        public EmployeeController()
        {
            _context = new companydbEntities();
        }
        // GET: Employee
        public ActionResult Index(int id = 1, string searchTerm="", int department = 0)
        {
            int limit = 5;
            var list = _context.employees.Where(e => e.name.Contains(searchTerm) && (e.department_id == department || department == 0)).
                OrderBy(e => e.id).Skip((id - 1) * limit).Take(limit).
                Select(e => new EmployeeModel
                {
                    Id_332 = e.id,
                    Name_332 = e.name,
                    phone_332 = e.phone,
                    departmentName = e.department.names,
                    position_332 = e.position,
                    email_332 = e.email,
                });
            ViewBag.ListEmployee = list;
            ViewBag.TotalPage = _context.employees.Where(e => e.name.Contains(searchTerm) && (e.department_id == department || department == 0)).Count();
            ViewBag.CurrentPage = id;

            ViewBag.ListDepartment = _context.departments.Select(e=>new DepartmentModel
            {
                id_332 = e.id,
                name_332 = e.names
            });
            return View();
        }

        public ActionResult Details(int id)
        {
            employee employee = _context.employees.SingleOrDefault(e => e.id == id);
            if (employee == null) return HttpNotFound();

            ViewBag.EmployeeDetails = new EmployeeModel
            {
                Id_332 = employee.id,
                email_332 = employee.email,
                phone_332 = employee.phone,
                position_332 = employee.position,
                imageUrl_332 = employee.avatar_url,
                departmentName = employee.department?.names,
                Name_332 = employee.name,
            };

            return View();
        }

        public ActionResult Add() {
            ViewBag.ListDepartment = _context.departments.Select(e => new DepartmentModel
            {
                id_332 = e.id,
                name_332 = e.names
            });
            return View();
        }

        [HttpPost]
        public ActionResult Add(EmployeeRequest model, HttpPostedFileBase imageFile)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                string uniqueFileName = fileName + "_" + Guid.NewGuid().ToString("N").Substring(0, 6) + extension;

                // Tạo thư mục Uploads nếu chưa tồn tại
                string path = Server.MapPath("~/Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Đường dẫn lưu file
                string fullPath = Path.Combine(path, uniqueFileName);
                imageFile.SaveAs(fullPath);


                var emp = new employee
                {
                    name = model.Name_332,
                    email = model.email_332,
                    phone = model.phone_332,
                    position = model.position_332,
                    department_id = model.departmentId_332,
                    avatar_url = "/Images/" + uniqueFileName, // đường dẫn tương đối cho hiển thị,
                    department = _context.departments.Find(model.departmentId_332)
                };

                _context.employees.Add(emp);
                _context.SaveChanges();

                TempData["Success"] = "Thêm nhân viên thành công!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Vui lòng chọn ảnh hợp lệ!";
            return RedirectToAction("Add"); 
        }

        public ActionResult ConfirmDelete(int id)
        {
            var employee = _context.employees.Find(id);
            if (employee == null)
            {
                TempData["Error"] = "Nhân viên không tồn tại!";
                return RedirectToAction("Index");
            }
            var employees = new EmployeeModel
            {
                Id_332 = employee.id,
                email_332 = employee.email,
                phone_332 = employee.phone,
                position_332 = employee.position,
                imageUrl_332 = employee.avatar_url,
                departmentName = employee.department?.names,
                Name_332 = employee.name,
            };
            return View(employees);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var employee = _context.employees.Find(id);
            if (employee == null)
            {
                TempData["Error"] = "Nhân viên không tồn tại!";
                return RedirectToAction("Index");
            }

            _context.employees.Remove(employee);
            _context.SaveChanges();

            TempData["Success"] = "Nhân viên đã được xóa thành công!";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var employee = _context.employees.Find(id);
            if (employee == null)
            {
                TempData["Error"] = "Nhân viên không tồn tại!";
                return RedirectToAction("Index");
            }
            var employees = new EmployeeModel
            {
                Id_332 = employee.id,
                email_332 = employee.email,
                phone_332 = employee.phone,
                position_332 = employee.position,
                imageUrl_332 = employee.avatar_url,
                departmentName = employee.department?.names,
                Name_332 = employee.name,
            };
            ViewBag.ListDepartment = _context.departments.Select(e => new DepartmentModel
            {
                id_332 = e.id,
                name_332 = e.names
            });
            return View(employees);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeRequest employee)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = _context.employees.Find(employee.id_332);
                if (existingEmployee == null)
                {
                    TempData["Error"] = "Nhân viên không tồn tại!";
                    return RedirectToAction("Index");
                }

                if (existingEmployee.name != employee.Name_332)
                    existingEmployee.name = employee.Name_332;

                if (existingEmployee.email != employee.email_332)
                    existingEmployee.email = employee.email_332;

                if (existingEmployee.phone != employee.phone_332)
                    existingEmployee.phone = employee.phone_332;

                if (existingEmployee.position != employee.position_332)
                    existingEmployee.position = employee.position_332;

                if (existingEmployee.department_id != employee.departmentId_332)
                {
                    existingEmployee.department_id = employee.departmentId_332;
                    existingEmployee.department = _context.departments.Find(employee.departmentId_332);
                }


                _context.SaveChanges();
                TempData["Success"] = "Nhân viên đã được chỉnh sửa thành công!";
                return RedirectToAction("Index");
            }

            return View(employee);
        }
    }
}