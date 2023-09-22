using Demo_WebAPI.Data;

namespace Demo_WebAPI.Interface
{
    public interface IEmployee
    {
        IEnumerable<Employee> getAllEmployee();
    }
}
