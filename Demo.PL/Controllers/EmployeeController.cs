using AutoMapper;
using Demo.BLL.InterFaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public IActionResult Index(string SearchValue = "")
        {
            IEnumerable<Employee> Employees;
            IEnumerable<EmployeeViewModel> employeeViewModels;
            if (string.IsNullOrEmpty(SearchValue))
            {
                 Employees = unitOfWork.employeeRepository.GetAll();
                employeeViewModels =mapper.Map<IEnumerable<EmployeeViewModel>>(Employees);
            }
            else {
                 Employees = unitOfWork.employeeRepository.Search(SearchValue );
                employeeViewModels = mapper.Map<IEnumerable<EmployeeViewModel>>(Employees);

            }
            return View(employeeViewModels);

        }

        public IActionResult Create()
        {
            ViewBag.Departments =unitOfWork.departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            //ModelState["Department"].ValidationState = ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                //Employee employee = new Employee
                //{
                //    Name = employeeViewModel.Name,
                //    Email = employeeViewModel.Email,
                //    Salary = employeeViewModel.Salary,
                //    Address = employeeViewModel.Address,
                //    DepartmentId = employeeViewModel.DepartmentId,
                //    HireDate = employeeViewModel.HireDate,
                //    IsActive = employeeViewModel.IsActive
                //};
                var employee = mapper.Map<Employee>(employeeViewModel);
                employee.ImageUrl =DocumentSettings.UploadFile(employeeViewModel.Image, "Images");
                unitOfWork.employeeRepository.Add(employee);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = unitOfWork.departmentRepository.GetAll();

            return View(employeeViewModel);
        }
        public IActionResult Details(int? id)
        {
            
                if (id == null) { return BadRequest(); }
                var Employee = unitOfWork.employeeRepository.GetById(id);
                if (Employee is null) { return NotFound(); }
                ViewBag.Departments = unitOfWork.departmentRepository.GetAll();

                return View(Employee);
            
          
        }


        public IActionResult Update(int? id)
        {

                if (id == null) { return BadRequest(); }
                var Employee = unitOfWork.employeeRepository.GetById(id);
                if (Employee is null) { return NotFound(); }
                var employeeViewModel = mapper.Map<EmployeeViewModel>(Employee);

                ViewBag.Departments = unitOfWork.departmentRepository.GetAll();
                return View(employeeViewModel);
            

        }
        [HttpPost]
        public IActionResult Update(int id, EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id) { }

            if (ModelState.IsValid)
                {
                    var employee = mapper.Map<Employee>(employeeViewModel);
                    DocumentSettings.DeleteFile("Images", employee.ImageUrl);
                    employee.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");
                    unitOfWork.employeeRepository.Update(employee);
                    unitOfWork.Complete();

                    return RedirectToAction(nameof(Index));
                }
            ViewBag.Departments = unitOfWork.departmentRepository.GetAll();

            return View(employeeViewModel);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var employee = unitOfWork.employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            DocumentSettings.DeleteFile("Images", employee.ImageUrl);

            unitOfWork.employeeRepository.Delete(employee);
            unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }
    }
}
