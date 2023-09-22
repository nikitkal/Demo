using Demo_WebAPI.Data;
using Demo_WebAPI.Interface;
using Demo_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo_WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private IEmployee _employee;

        public EmployeeController(IEmployee employee)
        {
            this._employee = employee;

        }


        [Authorize]
        [HttpGet]
        [Route("GetData")]
        public string GetData()
        {
            return "Authorized with Jwt";
        }

        
        [HttpGet]
        [Route("GetDetails")]
        public IEnumerable<Employee> GetDetails()
        {
            return _employee.getAllEmployee().ToList();

            //int num1 = 900;
            //int num2 = 0;
            //int result = num1 % num2;
            //return "Authorized with Jwt";
        }

        [Authorize]
        [HttpPost]
        [Route("AddUser")]
        public string AddUser(Users user)
        {
            return "User Added "+ user.Username;
        }
    }
}
