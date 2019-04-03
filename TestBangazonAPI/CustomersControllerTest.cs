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
        [Fact]
        public async Task Test_Get_One_Customers()
        {
            using (var client = new APIClientProvider().Client)
            {

                /*The Arrange section of a unit test method initializes objects and sets the value of the data that is passed to the method under test.*/

                /*The Act section invokes the method under test with the arranged parameters.*/

                var response = await client.GetAsync("/api/customers/1");

                string responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseBody);

                /*The Assert section verifies that the action of the method under test behaves as expected.*/

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customer.Id == 1);
            }
        }
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
    }
}
