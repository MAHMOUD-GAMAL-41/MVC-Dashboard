using Demo.BLL.InterFaces;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public IDepartmentRepository departmentRepository { get; set; }

        public IEmployeeRepository employeeRepository { get ; set ; }
        public UnitOfWork(AppDbContext context)
          
        {
            this.departmentRepository = new DepartmentRepository(context);
            this.employeeRepository = new EmployeeRepository(context);
            this.context = context;
        }

        public int Complete()
        {
            return context.SaveChanges();
        }
    }
}
