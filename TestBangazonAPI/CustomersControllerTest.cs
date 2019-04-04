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
    public class CustomersControllerTest
    {
        //GET ALL TEST
        [Fact]
        public async Task Test_Get_All_Customers()
        {
            using (var client = new APIClientProvider().Client)
            {

                /*The Arrange section of a unit test method initializes objects and sets the value of the data that is passed to the method under test.*/

                /*The Act section invokes the method under test with the arranged parameters.*/

                var response = await client.GetAsync("/api/customers");

                string responseBody = await response.Content.ReadAsStringAsync();
                var customerList = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

                /*The Assert section verifies that the action of the method under test behaves as expected.*/

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customerList.Count > 0);
            }
        }
        //GET ONE TEST
        [Fact]
        public async Task Test_Get_One_Customers()
        {
            using (var client = new APIClientProvider().Client)
            {

                //ARRANGE

                //ACT

                var response = await client.GetAsync("/api/customers/1");

                string responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseBody);

                //ASSERT

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customer.Id == 1);
            }
        }
        //POST NEW TEST
        [Fact]
        public async Task Test_Post_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE
                Customer customer = new Customer
                {
                    FirstName = "Rick",
                    LastName = "James"
                };

                // ACT
                var customerAsJSON = JsonConvert.SerializeObject(customer);

                var response = await client.PostAsync(
                    "/api/customers", new StringContent(customerAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                var newCustomer = JsonConvert.DeserializeObject<Customer>(responseBody);

                // ASSERT
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Rick", customer.FirstName);
            }
        }
        //EDIT EXISTING TEST
        [Fact]
        public async Task Test_Put_Customers()
        {
            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE
                string newFirstName = "Ricky";

                Customer modifiedCustomer = new Customer
                {
                    FirstName = newFirstName,
                    LastName = "Hansen"
                };

                // ACT
                var modifiedCustomerAsJSON = JsonConvert.SerializeObject(modifiedCustomer);

                var response = await client.PutAsync(
                    "/api/customers/1", new StringContent(modifiedCustomerAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                var getModifiedCustomer = await client.GetAsync("/api/customers/1");
                getModifiedCustomer.EnsureSuccessStatusCode();

                string getModifiedCustomerContent = await getModifiedCustomer.Content.ReadAsStringAsync();
                Customer updatedCustomer = JsonConvert.DeserializeObject<Customer>(getModifiedCustomerContent);

                // ASSERT

                Assert.Equal(HttpStatusCode.OK, getModifiedCustomer.StatusCode);
                Assert.Equal(newFirstName, updatedCustomer.FirstName);
            }
        }
        // ----------DELETE for Payment Types---------
        [Fact]
        public async Task Test_Delete_Customers()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/customers/3");

                string responseBody = await response.Content.ReadAsStringAsync();
                var deleteCustomers = JsonConvert.DeserializeObject<Customer>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
