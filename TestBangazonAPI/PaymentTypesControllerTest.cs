using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        [Fact]
        public async Task Test_Get_One_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {

                // ARRANGE

                var response = await client.GetAsync("/api/paymenttypes/1");

                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                // ACT

                // ASSERT

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentType.Id == 1);
            }
        }
        // ----------POST for Payment Types---------
        /*
        [Fact]
        public async Task Test_Post_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE
                PaymentType testType = new PaymentType
                {
                    AcctNumber = 1998832,
                    Name = "VenmoCard",
                    CustomerId = 1
                };

                // ACT
                var testTypeAsJSON = JsonConvert.SerializeObject(testType);

                var response = await client.PostAsync(
                    "/api/paymenttypes", new StringContent(testTypeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                var newPaymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                // ASSERT
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("VenmoCard", newPaymentType.Name);
            }
        }
        */
        // ----------PUT for Payment Types---------
        [Fact]
        public async Task Test_Put_PaymentTypes()
        {


            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE
                string newName = "MasterKard";

                PaymentType modifiedType = new PaymentType
                {
                    AcctNumber = 1998832,
                    Name = newName,
                    CustomerId = 1
                };

                // ACT
                var modifiedTypeAsJSON = JsonConvert.SerializeObject(modifiedType);

                var response = await client.PutAsync(
                    "/api/paymenttypes/1", new StringContent(modifiedTypeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                var getModifiedType = await client.GetAsync("/api/paymenttypes/1");
                getModifiedType.EnsureSuccessStatusCode();

                string getModifiedTypeContent = await getModifiedType.Content.ReadAsStringAsync();
                PaymentType updatedType = JsonConvert.DeserializeObject<PaymentType>(getModifiedTypeContent);

                // ASSERT

                Assert.Equal(HttpStatusCode.OK, getModifiedType.StatusCode);
                Assert.Equal(newName, updatedType.Name);
            }
        }

        /*
                // ----------DELETE for Payment Types---------
        [Fact]
        public async Task Test_Delete_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE

                // ACT

                // ASSERT
            }
        }
        */
    }
}
