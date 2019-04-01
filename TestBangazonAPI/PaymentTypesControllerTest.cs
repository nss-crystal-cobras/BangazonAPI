using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestBangazonAPI.Test;
using Xunit;

namespace TestBangazonAPI
{
    public class PaymentTypesControllerTest
    {
        [Fact]
        // --------GET ALL for Payment Types---------

        public async Task Test_Get_All_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {

                /*The Arrange section of a unit test method initializes objects and sets the value of the data that is passed to the method under test.*/

                /*The Act section invokes the method under test with the arranged parameters.*/

                var response = await client.GetAsync("/api/paymenttypes");

                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentTypeList = JsonConvert.DeserializeObject<List<PaymentType>>(responseBody);

                /*The Assert section verifies that the action of the method under test behaves as expected.*/

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentTypeList.Count > 0);
            }
        }

        // ----------GET SINGLE for Payment Types---------

        public async Task Test_Get_One_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE

                // ACT

                // ASSERT
            }
        }

        // ----------POST for Payment Types---------

        public async Task Test_Post_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE

                // ACT

                // ASSERT
            }
        }

        // ----------PUT for Payment Types---------

        public async Task Test_Put_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE

                // ACT

                // ASSERT
            }
        }
    }        
}
