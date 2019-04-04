using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class EmployeeTest
    {
        [Fact]
        public async Task Test_Create_Employee()
        {
            using (var client = new APIClientProvider().Client)
            {   //creating new test object
                Employee employee = new Employee
                {
                    FirstName = "Hernando",
                    LastName = "Rivera",
                    IsSupervisor = false,
                    DepartmentId = 1,
                    //Department = new Department
                    //{
                    //    Id = 1,
                    //    Name = "Accounting",
                    //    Budget = 400000
                    //},
                    //Computer = new Computer
                    //{
                    //    Id = 1,
                    //    Make = "MacBook Pro",
                    //    Manufacturer = "Apple"
                    //}
                };
                // json conversion of the object to a new object.
                var employeeAsJSON = JsonConvert.SerializeObject(employee);

                //post the test object and creating a variable that will 
                var response = await client.PostAsync(
                    "/api/employees",
                    new StringContent(employeeAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newEmployee = JsonConvert.DeserializeObject<Employee>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Hernando", newEmployee.FirstName);
                Assert.Equal("Rivera", newEmployee.LastName);
                Assert.False(newEmployee.IsSupervisor);
                //Assert.Equal(1, newEmployee.DepartmentId);
                //Assert.Equal(1, newEmployee.Department.Id);
                //Assert.Equal("Accounting", newEmployee.Department.Name);
                //Assert.Equal(400000, newEmployee.Department.Budget);
                //Assert.Equal(1, newEmployee.Computer.Id);
                //Assert.Equal("MacBook Pro", newEmployee.Computer.Make);
                //Assert.Equal("Apple", newEmployee.Computer.Manufacturer);
            }
        }

        [Fact]
        public async Task Test_Get_All_Employees()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/employees");

                string responseBody = await response.Content.ReadAsStringAsync();
                var employeeList = JsonConvert.DeserializeObject<List<Employee>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(employeeList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Edit_Customer()
        {
            string newName = "Testie";

            using (var client = new APIClientProvider().Client)
            {
                Employee editedEmployee = new Employee
                {
                    Id = 1,
                    FirstName = "Hernando",
                    LastName = newName,
                    IsSupervisor = false,
                    DepartmentId = 1,
                    //Department = new Department
                    //{
                    //    Id = 1,
                    //    Name = "Accounting",
                    //    Budget = 400000
                    //},
                    //Computer = new Computer
                    //{
                    //    Id = 1,
                    //    Make = "MacBook Pro",
                    //    Manufacturer = "Apple"
                    //}
                };
                var editedEmployeeAsJSON = JsonConvert.SerializeObject(editedEmployee);

                var response = await client.PutAsync(
                    "/api/employees/1",
                    new StringContent(editedEmployeeAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getTest = await client.GetAsync("/api/employees/1");
                getTest.EnsureSuccessStatusCode();

                string testItem = await getTest.Content.ReadAsStringAsync();
                Employee newLastName = JsonConvert.DeserializeObject<Employee>(testItem);

                Assert.Equal(HttpStatusCode.OK, getTest.StatusCode);
                Assert.Equal(newName, newLastName.LastName);
            }
        }
    }
}
