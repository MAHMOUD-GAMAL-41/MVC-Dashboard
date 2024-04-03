using AutoMapper;
using Demo.BLL.InterFaces;
using Demo.BLL.Repositories;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        //private readonly IDepartmentRepository departmentRepository;
        private readonly ILogger<DepartmentController> logger;
        private readonly IMapper mapper;

        public DepartmentController(
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            ILogger<DepartmentController> logger,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            //this.departmentRepository = departmentRepository;
            this.logger = logger;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            var departmesnts = unitOfWork.departmentRepository.GetAll();
            IEnumerable<DepartmentViewModel>  departmentViewModel = mapper.Map<IEnumerable<DepartmentViewModel>>(departmesnts);

            //ViewBag.Message = "Hello From View Bag ";
            //ViewData["MessageData"] = "Hello From View Data";
            return View(departmentViewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentViewModel)
        {
           
            if (ModelState.IsValid)
            {
                var department = mapper.Map<Department>(departmentViewModel);

                unitOfWork.departmentRepository.Add(department);
                unitOfWork.Complete();
                TempData["MessageData"] = "Department Added Successfully";
            return RedirectToAction(nameof(Index));
            }
            else return View(departmentViewModel);
        }
        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null) { return BadRequest(); }
                var department = unitOfWork.departmentRepository.GetById(id);
                if (department is null) { return NotFound(); }
                return View(department);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        public IActionResult Update(int? id)
        {
            try
            {
                if (id == null) { return BadRequest(); }
                var department = unitOfWork.departmentRepository.GetById(id);
                if (department is null) { return NotFound(); }

                return View(department);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public IActionResult Update(int id,Department department)
        {
            if(id !=department.Id) { }
            try
            {
                if (ModelState.IsValid)
                {

                    unitOfWork.departmentRepository.Update(department);
                    unitOfWork.Complete();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(department);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var department = unitOfWork.departmentRepository.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            unitOfWork.departmentRepository.Delete(department);
            unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }
    }
}
