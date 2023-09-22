using Demo_WebAPI.Data;
using Demo_WebAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo_WebAPI.Services
{
    public class EmployeeService:IEmployee
    {
        //private Employee _emp;
        private DbSet<Employee> _emp;
        public EmployeeService(DbContext context)
        {
           _emp= context.Set<Employee>();

        }
        public IEnumerable<Employee> getAllEmployee() {
            return _emp.AsNoTracking().AsEnumerable<Employee>();
        }
    }
}
