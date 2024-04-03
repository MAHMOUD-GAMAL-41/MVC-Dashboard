using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.InterFaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IEnumerable<Employee> GetEmployeesByDepartmentName(string departmentName);
        IEnumerable<Employee> Search(string name);

        //Employee GetById(int id);
        //IEnumerable<Employee> GetAll();

        //int Add(Employee employee);
        //int Update(Employee employee);
        //int Delete(Employee employee);

    }
}
