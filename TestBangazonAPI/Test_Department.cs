
using Newtonsoft.Json;
using BangazonAPI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class Test_Department
    {
        //GetAll
        [Fact]
        public async Task Test_Get_All_Department()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/Department");
                string responseBody = await response.Content.ReadAsStringAsync();
                var DepartmentList = JsonConvert.DeserializeObject<List<Department>>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(DepartmentList.Count > 0);
            }

        }
        //Get One
        [Fact]
        public async Task Test_Get_One_Department()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/Department/2");
                string responseBody = await response.Content.ReadAsStringAsync();
                var Department = JsonConvert.DeserializeObject<Department>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(Department.Id == 2);
            }

        }

        //Post
        [Fact]
        public async Task Test_Insert_A_Department()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {

                Department Automotive = new Department
                {
                    Name = "Automotive",
                    Budget = 450000


                };


                var AutomotiveAsJSON = JsonConvert.SerializeObject(Automotive);

                var response = await client.PostAsync(
                    "/api/Department",
                    new StringContent(AutomotiveAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newAutomotive = JsonConvert.DeserializeObject<Department>(responseBody);


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Automotive", newAutomotive.Name);

            }
        }

        //put 
        [Fact]
        public async Task Test_Modify_Department()
        {
            // New productType to change to and test
            string newName = "LegalTest";
            //int newBudget = 3005000;

            using (HttpClient client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                */
                Department modifiedDepartment = new Department
                {

                    Name = newName,
                    Budget = 300000


                };
                var modifiedDepartmentAsJSON = JsonConvert.SerializeObject(modifiedDepartment);

                var response = await client.PutAsync(
                    "/api/Department/4",
                    new StringContent(modifiedDepartmentAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);


                /*
                    GET section
                    Verify that the PUT operation was successful
                */
                HttpResponseMessage getmodifiedDepartment = await client.GetAsync("/api/Department/4");
                getmodifiedDepartment.EnsureSuccessStatusCode();

                string getmodifiedDepartmentBody = await getmodifiedDepartment.Content.ReadAsStringAsync();
                Department newDepartment = JsonConvert.DeserializeObject<Department>(getmodifiedDepartmentBody);

                Assert.Equal(HttpStatusCode.OK, getmodifiedDepartment.StatusCode);
                Assert.Equal(newName, newDepartment.Name);
                //Assert.Equal(newBudget, newDepartment.Budget);
            }
        }

        //Delete
        [Fact]
        public async Task Test_Delete_Department()
        {

            using (HttpClient client = new APIClientProvider().Client)
            {

                HttpResponseMessage response = await client.DeleteAsync("/api/Department/7");


                string responseBody = await response.Content.ReadAsStringAsync();
               Department Department = JsonConvert.DeserializeObject<Department>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            }

        }

    }
}


