using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Task3_4.Models;
using Task3_4.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Task3_4
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ServiceFilter(typeof(CustomAuthFilter))]
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> _employees;

        public EmployeeController()
        {
            if (_employees == null)
                _employees = GetStandardEmployeeList();
        }

        private List<Employee> GetStandardEmployeeList()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "John",
                    Salary = 50000,
                    Permanent = true,
                    Department = new Department { Id = 1, Name = "HR" },
                    Skills = new List<Skill> { new Skill { Id = 1, Name = "C#" } },
                    DateOfBirth = new DateTime(1990, 1, 1)
                },
                new Employee
                {
                    Id = 2,
                    Name = "Jane",
                    Salary = 60000,
                    Permanent = false,
                    Department = new Department { Id = 2, Name = "IT" },
                    Skills = new List<Skill> { new Skill { Id = 2, Name = "SQL" } },
                    DateOfBirth = new DateTime(1992, 2, 2)
                }
            };
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Employee>), 200)]
        [ProducesResponseType(500)]
        public ActionResult<List<Employee>> Get()
        {
            // Uncomment to test exception filter
            // throw new Exception("Test exception");
            return Ok(_employees);
        }

        [HttpPost]
        public ActionResult<Employee> Post([FromBody] Employee employee)
        {
            _employees.Add(employee);
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public ActionResult<Employee> Put(int id, [FromBody] Employee employee)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee id");
            }
            var emp = _employees.FirstOrDefault(e => e.Id == id);
            if (emp == null)
            {
                return BadRequest("Invalid employee id");
            }
            emp.Name = employee.Name;
            emp.Salary = employee.Salary;
            emp.Permanent = employee.Permanent;
            emp.Department = employee.Department;
            emp.Skills = employee.Skills;
            emp.DateOfBirth = employee.DateOfBirth;
            return Ok(emp);
        }
    }
}
