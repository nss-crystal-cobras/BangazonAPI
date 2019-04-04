using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BangazonAPI.Models;
namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private string budget;

        public DepartmentController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: api/Department
        [HttpGet]
        public IEnumerable<Department> Get(string _include, string _filter, string _gt, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (_include == "employees")
                    {
                        cmd.CommandText = @"select d.Id as DepartmentId,
                                               d.[Name],
                                               d.Budget,
                                               e.Id as EmployeeId,
                                               e.FirstName as FirstName,
                                               e.LastName as LastName,
                                               e.IsSuperVisor as Supervisor
                                             
                                      
                                            from department d
                                               left join employee e on d.Id = e.DepartmentId
                                               WHERE 1 = 1";
                    }
                    else if (_filter == "budget" && _gt =="300000")
                    {


                        cmd.CommandText = @"select d.Id as DepartmentId,
                                               d.[Name],
                                               d.Budget
                                               from department d
                                                WHERE d.Budget > 300000";


                    }

                    else
                    {
                        cmd.CommandText = @"select d.Id as DepartmentId,
                                               d.[Name],
                                               d.Budget
                                               from department d
                                                WHERE 1 = 1";


                    }



                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Department> departments = new Dictionary<int, Department>();
                    while (reader.Read())
                    {
                        int DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"));
                        if (!departments.ContainsKey(DepartmentId))
                        {
                            if (_filter == "budget" && _gt == "300000")
                            {
                                Department budgetGt300000 = new Department
                                {
                                    Id = DepartmentId,

                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Budget = reader.GetInt32(reader.GetOrdinal("Budget")),

                                };
                                departments.Add(DepartmentId, budgetGt300000);
                            }
                            else
                            {
                                Department newDepartment = new Department
                                {
                                    Id = DepartmentId,

                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Budget = reader.GetInt32(reader.GetOrdinal("Budget")),

                                };

                                departments.Add(DepartmentId, newDepartment);
                            }
                        }

                        if (_include == "employees")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                            {
                                Department currentDepartment = departments[DepartmentId];
                                currentDepartment.employees.Add(
                                    new Employee
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                        IsSupervisor = reader.GetBoolean(reader.GetOrdinal("Supervisor")),
                                        DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                                    });
                                
                            }
                        }
                    }


                    reader.Close();

                    return departments.Values.ToList();
                }
            }
        }

        // GET: api/Department/5
       

        [HttpGet("{id}", Name = "GetSingleDepartment")]
        public Department Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select d.Id as DepartmentId,
                                               d.[Name],
                                               d.Budget,
                                               e.Id as EmployeeId,
                                               e.FirstName as FirstName,
                                               e.LastName as LastName,
                                               e.IsSuperVisor as Supervisor
                                             
                                      
                                            from department d
                                               left join employee e on d.Id = e.DepartmentId
                                               WHERE d.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Department department = null;
                    //Dictionary<int, Department> department = new Dictionary<int, Department>();
                    if (reader.Read())
                    {
                        if (department == null)
                        {
                            department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget")),

                            };
                        }
                    }

                  /*  if (!reader.IsDBNull(reader.GetOrdinal("DepartmentId")))
                    {
                        int departmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"));
                        if (!department.employees.Any(e => e.Id == departmentId))
                        {
                            Employee employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                IsSupervisor = reader.GetBoolean(reader.GetOrdinal("Supervisor")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                            };
                            department.employees.Add(employee);
                        }
                    }*/

                    reader.Close();
                    return department;
                }
            }
        }


        // POST: api/Department
        [HttpPost]
        public ActionResult Post([FromBody] Department newDepartment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"insert into Department ([Name], Budget)
                                     OUTPUT INSERTED.Id

                                         values (@Name, @Budget)";

                    cmd.Parameters.Add(new SqlParameter("@Name", newDepartment.Name));
                    cmd.Parameters.Add(new SqlParameter("@Budget", newDepartment.Budget));
                    
                    int newId = (int)cmd.ExecuteScalar();
                    newDepartment.Id = newId;
                    return CreatedAtRoute("GetSingleDepartment", new { id = newId }, newDepartment);
                }
            }
        }

        // PUT: api/Department/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Department department)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE department 
                                           SET Name = @name, 
                                               Budget = @budget
                                               
                                         WHERE id = @id";
                    cmd.Parameters.Add(new SqlParameter("@name", department.Name));
                    cmd.Parameters.Add(new SqlParameter("@budget", department.Budget));
                    
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Department WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
