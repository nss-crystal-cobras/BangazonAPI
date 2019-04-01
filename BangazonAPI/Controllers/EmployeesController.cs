using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public EmployeesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            }
        }
        //GET: api/Employees
        [HttpGet]
        public IActionResult Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.id, e.firstname, e.lastname, e.departmentId,
                                                d.id, d.[name] as departmentName, d.Budget as departmentBudget,
                                                c.id, c.Make as computerMake, c.Manufacturer as computerManufacturer,
                                          FROM Employee e 
                                               left join Department d on e.id = d.employeeId
                                               left join Computer c on e.id = c.employeeId
                                         where e.id = @id;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                            LastName = reader.GetString(reader.GetOrdinal("lastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentId")),
                            Department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("departmentId")),
                                Name = reader.GetString(reader.GetOrdinal("departmentName")),
                                Budget = reader.GetInt32(reader.GetOrdinal("departmentBudget"))
                            },
                            Computer = new Computer
                            {
                            Id = reader.GetInt32(reader.GetOrdinal("computerId")),
                            Make = reader.GetString(reader.GetOrdinal("computerMake")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("computerManufacturer"))
                        }
                        };

                        employees.Add(employee);
                    }
               
                    reader.Close();
                    if (employees.Count == 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(employees);
                    }
                }
            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}", Name = "GetSingleEmployee")]
        public Employee Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select e.id, e.firstName, e.lastName, 
                                               d.id AS departmentId, d.[name] AS departmentName, 
                                               c.id AS computerId, c.Make AS computerManufacturer,
                                               c.Make AS computerMake
                                          FROM Employee e 
                                               left join Department d on e.id = d.employeeId
                                               left join Computer c on e.id = c.employeeId
                                         where e.id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee employee = null;
                    while (reader.Read())
                    {
                        if (employee == null)
                        {
                            employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                LastName = reader.GetString(reader.GetOrdinal("lastName")),
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("departmentId")))
                        {
                            int departmentId = reader.GetInt32(reader.GetOrdinal("departmentId"));
                            if (!employee.Departments.Any(d => d.Id == departmentId))
                            {
                                Department department = new Department
                                {
                                    Id = departmentId,
                                    Name= reader.GetString(reader.GetOrdinal("[name]")),

                                    EmployeeId = employee.Id
                                };
                                employee.Departments.Add(department);
                            }
                        }


                        if (!reader.IsDBNull(reader.GetOrdinal("computerId")))
                        {
                            int computerId = reader.GetInt32(reader.GetOrdinal("computerId"));
                            if (!employee.Computers.Any(c => c.Id == computerId))
                            {
                                Computer computer = new Computer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("computerId")),
                                    Manufacturer = reader.GetString(reader.GetOrdinal("computerManufacturer")),
                                    Make = reader.GetString(reader.GetOrdinal("computerMake")),
                                    
                                    EmployeeId = employee.Id
                                };

                                employee.Computers.Add(computer);
                            }
                        }
                    }


                    reader.Close();
                    return employee;
                }
            }
        }

        // POST: api/Employees
        [HttpPost]
        public void Post([FromBody] string value)
        {


        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {


        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
