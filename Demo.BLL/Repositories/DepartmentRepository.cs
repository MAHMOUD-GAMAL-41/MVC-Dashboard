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
    public class DepartmentRepository :GenericRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }
        //public IEnumerable<Department> GetAll() => _context.departments.ToList();
        //public Department GetById(int? id) => _context.departments.FirstOrDefault(X => X.Id == id);

        //public int Add(Department department)
        //{
        //    _context.departments.Add(department);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Department department)
        //{
        //    _context.departments.Remove(department);
        //    return _context.SaveChanges();
        //}


        //public int Update(Department department)
        //{
        //    _context.departments.Update(department);
        //    return _context.SaveChanges();
        //}
    }
}
