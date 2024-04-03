using Demo.BLL.InterFaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetEmployeesByDepartmentName(string departmentName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> Search(string name)
        {
            var result = _context.employees.Where(
                employee=>
                employee.Name.Trim().ToLower().Contains(name.Trim().ToLower()) ||
                employee.Email.Trim().ToLower().Contains(name.Trim().ToLower())
                );
            return result;
        }
        //public IEnumerable<Employee> GetAll()=>_context.employees.ToList();
        //public Employee GetById(int id) => _context.employees.FirstOrDefault(X => X.Id == id);

        //public int Add(Employee employee)
        //{
        //    _context.employees.Add(employee);
        //   return _context.SaveChanges();
        //}

        //public int Delete(Employee employee)
        //{
        //    _context.employees.Remove(employee);
        //    return _context.SaveChanges();
        //}


        //public int Update(Employee employee)
        //{
        //    _context.employees.Update(employee);
        //    return _context.SaveChanges();
        //}
    }
}
