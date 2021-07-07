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
       /* [HttpGet]
        public IActionResult Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.id, e.firstname, e.lastname, e.departmentId,
                                                d.id, d.[name] AS departmentName, d.Budget AS departmentBudget,
                                                c.id as computerId, c.make as computerMake, c.manufacturer as computerManufacturer
                                          FROM Employee e 
                                               left join Department d on e.departmentId = d.id
                                               left join ComputerEmployee ec on e.id = ec.EmployeeId
                                               left join Computer c on c.id = ec.ComputerId;";

                   
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
        }*/

        // GET: api/Employees/5
        [HttpGet("{id}", Name = "GetSingleEmployee")]
        public IActionResult Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.id, e.firstname, e.lastname, e.departmentId,
                                                d.id, d.[name] AS departmentName, d.Budget AS departmentBudget,
                                                c.id as computerId, c.make as computerMake, c.manufacturer as computerManufacturer
                                          FROM Employee e 
                                               left join Department d on e.departmentId = d.id
                                               left join ComputerEmployee ec on e.id = ec.EmployeeId
                                               left join Computer c on c.id = ec.ComputerId
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
                        }

                    }

                    reader.Close();
                 
                   
                    return Ok(employee);
                    
                }
            }
        }

        // POST: api/Employees
        [HttpPost]
        public ActionResult Post([FromBody] Employee newEmployee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO employee (firstname, lastname, departmentId, isSupervisor)
                                             OUTPUT INSERTED.Id
                                             VALUES (@firstname, @lastname, @departmentId, @isSupervisor)";
                    cmd.Parameters.Add(new SqlParameter("@firstname", newEmployee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastname", newEmployee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@departmentId", newEmployee.DepartmentId));
                    cmd.Parameters.Add(new SqlParameter("@isSupervisor", newEmployee.IsSupervisor));

                    int newId = (int)cmd.ExecuteScalar();
                    newEmployee.Id = newId;
                    return CreatedAtRoute("GetSingleEmployee", new { id = newId }, newEmployee);
                }
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Employee newEmployee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO employee (firstname, lastname, departmentId, isSupervisor)
                                             OUTPUT INSERTED.Id
                                             VALUES (@firstname, @lastname, @departmentId, @isSupervisor)";
                    cmd.Parameters.Add(new SqlParameter("@firstname", newEmployee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastname", newEmployee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@departmentId", newEmployee.DepartmentId));
                    cmd.Parameters.Add(new SqlParameter("@isSupervisor", newEmployee.IsSupervisor));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
