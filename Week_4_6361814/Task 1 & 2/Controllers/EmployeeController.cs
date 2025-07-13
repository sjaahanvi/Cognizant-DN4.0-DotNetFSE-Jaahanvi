using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Task1_2.Controllers
{
    [ApiController]
    [Route("api/emp")]
    public class EmployeeController : ControllerBase
    {
        private static readonly List<Employee> Employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Alice", Department = "HR" },
            new Employee { Id = 2, Name = "Bob", Department = "IT" },
            new Employee { Id = 3, Name = "Charlie", Department = "Finance" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            return Employees;
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
    }
}
