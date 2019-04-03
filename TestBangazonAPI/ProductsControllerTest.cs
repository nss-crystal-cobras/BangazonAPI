using System;
using System.Collections.Generic;
using System.Text;

namespace TestBangazonAPI
{
    class ProductsControllerTest
    {

         {
        [Fact]
        public async Task Get_All_Products_Returns_Some_Products()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/products");

                response.EnsureSuccessStatusCode();
            }
        }
    }






}
}
