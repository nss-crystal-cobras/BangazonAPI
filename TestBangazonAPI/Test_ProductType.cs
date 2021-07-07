
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
    public class Test_ProductType
    {
        //GetAll
        [Fact]
        public async Task Test_Get_All_ProductType()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/ProductTypes");
                string responseBody = await response.Content.ReadAsStringAsync();
                var ProductTypeList = JsonConvert.DeserializeObject<List<ProductType>>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(ProductTypeList.Count > 0);
            }

        }
        //Get One
        [Fact]
        public async Task Test_Get_One_ProductType()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/ProductTypes/2");
                string responseBody = await response.Content.ReadAsStringAsync();
                var ProductType = JsonConvert.DeserializeObject<ProductType>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(ProductType.Id == 2);
            }

        }

        //Post
        [Fact]
        public async Task Test_Insert_A_ProductType()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {

                ProductType Frozen = new ProductType
                {
                    Name = "Frozen"

                    
                };


                var FrozenAsJSON = JsonConvert.SerializeObject(Frozen);

                var response = await client.PostAsync(
                    "/api/ProductTypes",
                    new StringContent(FrozenAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newFrozen = JsonConvert.DeserializeObject<ProductType>(responseBody);


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Frozen", newFrozen.Name);
                
            }
        }

        //put 
        [Fact]
        public async Task Test_Modify_ProductType()
        {
            // New productType to change to and test
            string newProductType = "FloralAndPlants";

            using (HttpClient client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                */
                ProductType modifiedProductType = new ProductType
                {
                    
                    Name = newProductType
                   
                };
                var modifiedProductTypeAsJSON = JsonConvert.SerializeObject(modifiedProductType);

                var response = await client.PutAsync(
                    "/api/ProductTypes/1",
                    new StringContent(modifiedProductTypeAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


                /*
                    GET section
                    Verify that the PUT operation was successful
                */
                HttpResponseMessage getmodifiedProductType = await client.GetAsync("/api/ProductTypes/1");
                getmodifiedProductType.EnsureSuccessStatusCode();

                string getmodifiedProductTypeBody = await getmodifiedProductType.Content.ReadAsStringAsync();
                ProductType newType = JsonConvert.DeserializeObject<ProductType>(getmodifiedProductTypeBody);

                Assert.Equal(HttpStatusCode.OK, getmodifiedProductType.StatusCode);
                Assert.Equal(newProductType, newType.Name);
            }
        }

        //Delete
        [Fact]
        public async Task Test_Delete_ProductType()
        {

            using (HttpClient client = new APIClientProvider().Client)
            {

                HttpResponseMessage response = await client.DeleteAsync("/api/ProductTypes/19");


                string responseBody = await response.Content.ReadAsStringAsync();
                ProductType ProductType = JsonConvert.DeserializeObject<ProductType>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            }

        }

    }
}


