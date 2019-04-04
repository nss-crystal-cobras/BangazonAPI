using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestBangazonAPI;
using Xunit;



namespace TestBangazonAPI
{
    public class ProductsControllerTest
    {
        [Fact]
        // --------GET ALL for Products---------

        public async Task Test_Get_All_Products()
        {
            using (var client = new APIClientProvider().Client)
            {

                /*The Arrange section of a unit test method initializes objects and sets the value of the data that is passed to the method under test.*/

                /*The Act section invokes the method under test with the arranged parameters.*/

                var response = await client.GetAsync("/api/products");

                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentTypeList = JsonConvert.DeserializeObject<List<Product>>(responseBody);

                /*The Assert section verifies that the action of the method under test behaves as expected.*/

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentTypeList.Count > 0);
            }
        }

        // ----------GET SINGLE for Payment Types---------
        [Fact]
        public async Task Test_Get_One_Product()
        {
            using (var client = new APIClientProvider().Client)
            {

                // ARRANGE

                var response = await client.GetAsync("/api/products/1");

                string responseBody = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(responseBody);

                // ACT

                // ASSERT

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(product.Id == 1);
            }
        }
        // ----------POST for Products---------

        [Fact]
        public async Task Test_Post_Products()
        {
            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE
                
                Product testProduct = new Product
                {
                    Price = 4976522,
                    Title = "Bridge",
                    Description = "Singing River",
                    Quantity = 1
                };


                // ACT
                var testTypeAsJSON = JsonConvert.SerializeObject(testProduct);

                var response = await client.PostAsync(
                    "/api/products", new StringContent(testTypeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                var newlyCreatedProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                // ASSERT
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Bridge", newlyCreatedProduct.Title);
            }
        }

        // ----------PUT for Proucts---------
        [Fact]
        public async Task Test_Put_Products()
        {


            using (var client = new APIClientProvider().Client)
            {
                // ARRANGE
                string newName = "Pascagoula River Bridge";

                Product modifiedProduct = new Product
                {
                    Price = 4976522,
                    Title = "Pascagoula River Bridge",
                    Description = "Singing River",
                    Quantity = 1
                };

                // ACT
                var modifiedTypeAsJSON = JsonConvert.SerializeObject(modifiedProduct);

                var response = await client.PutAsync(
                    "/api/products/1", new StringContent(modifiedTypeAsJSON, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                var getModifiedType = await client.GetAsync("/api/products/1");
                getModifiedType.EnsureSuccessStatusCode();

                string getModifiedTypeContent = await getModifiedType.Content.ReadAsStringAsync();
                Product updatedProduct = JsonConvert.DeserializeObject<Product>(getModifiedTypeContent);

                // ASSERT

                Assert.Equal(HttpStatusCode.OK, getModifiedType.StatusCode);
                Assert.Equal(newName, updatedProduct.Title);
            }
        }


        // ----------DELETE  Product---------
        /* [Fact]
         public async Task Test_Delete_Product()
         {

             using (var client = new APIClientProvider().Client)
             {
                 var response = await client.DeleteAsync("/api/products/2");

                 string responseBody = await response.Content.ReadAsStringAsync();
                 var deleteProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                 /*
                     ASSERT

                 Assert.Equal(HttpStatusCode.OK, response.StatusCode);
             }
         }*/


        /*this one heavily modeled after Russells team
    [Fact]
    public async Task Test_Remove_Product()
    {
        using (var client = new APIClientProvider().Client)
        {
            var productGetInitialResponse = await client.GetAsync("api/products");
            string initialResponseBody = await productGetInitialResponse.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<List<Product>>(initialResponseBody);
            Assert.Equal(HttpStatusCode.OK, productGetInitialResponse.StatusCode);
            int removeLastObject = productList.Count - 1;
            var productObject = productList[removeLastObject];

            var response = await client.DeleteAsync($"api/products/{productObject.Id}");

            string responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var getProduct = await client.GetAsync($"api/products/{productObject.Id}");
            getProduct.EnsureSuccessStatusCode();

            string getProductBody = await getProduct.Content.ReadAsStringAsync();
            Product newProduct = JsonConvert.DeserializeObject<Product>(getProductBody);

            Assert.Equal(HttpStatusCode.NoContent, getProduct.StatusCode);
        }
    }*/


        /*This one from Cole B*/
        public async Task Test_Remove_Product()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/product/3");

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }


    }
}
