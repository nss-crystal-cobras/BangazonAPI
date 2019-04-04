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
    public class ProductsController : ControllerBase
    {

        private readonly IConfiguration _config;

        public ProductsController(IConfiguration config)
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






        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, productTypeId, Price , Title, Description, Quantity
                                        FROM Product ";

        SqlDataReader reader = cmd.ExecuteReader();
                    List<Product> products = new List<Product>();

                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("productTypeId")),
                            Price = reader.GetInt32(reader.GetOrdinal("Price")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        };
                        products.Add(product);
                    }
                    reader.Close();

                    return Ok(products);
                }
            }
        }

       
        
        // GET: api/Products/5

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, productTypeId, Price , Title, Description, Quantity, CustomerId
                                        FROM Product 
                                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Product product = null;

                    if (reader.Read())
                    {
                         product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("productTypeId")),
                            Price = reader.GetInt32(reader.GetOrdinal("Price")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))

                        };
                    }
                    reader.Close();

                    return Ok(product);
                }
            }
        }




        
        
        // CODE FOR CREATING A Product

        // POST: api/Product

        [HttpPost]
        public ActionResult Post([FromBody] Product newProduct)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO product (productTypeId, Price , Title, Description, Quantity, CustomerId)
                                             OUTPUT INSERTED.Id
                                             VALUES (@ProductTypeId, @Price, @Title, @Description, @Quantity, @CustomerId)";
                    cmd.Parameters.Add(new SqlParameter("@productTypeId", newProduct.ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@Price", newProduct.Price));
                    cmd.Parameters.Add(new SqlParameter("@Title", newProduct.Title));
                    cmd.Parameters.Add(new SqlParameter("@Description", newProduct.Description));
                    cmd.Parameters.Add(new SqlParameter("@Quantity", newProduct.Quantity));
                    cmd.Parameters.Add(new SqlParameter("@CustomerId", newProduct.CustomerId));

                    int newId = (int)cmd.ExecuteScalar();
                    newProduct.Id = newId;
                    return CreatedAtRoute("GetProduct", new { id = newId }, newProduct);
                }
            }
        }




        // PUT: api/Products/5


        /*
                [HttpPut("{id}")]
                public void Put(int id, [FromBody] Product product)
                {
                    using (SqlConnection conn = Connection)
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"UPDATE product 
                                                   SET productTypeId = @productTypeId,
                                                                Price = @Price, 
                                                                Title = @Title,
                                                                Description = @Description, 
                                                                Quantity = @Quantity,
                                                                CustomerId = @CustomerId
                                                 WHERE id = @id;";
                            cmd.Parameters.Add(new SqlParameter("@productTypeId", product.ProductTypeId));
                            cmd.Parameters.Add(new SqlParameter("@Price", product.Price));
                            cmd.Parameters.Add(new SqlParameter("@Title", product.Title));
                            cmd.Parameters.Add(new SqlParameter("@Description", product.Description));
                            cmd.Parameters.Add(new SqlParameter("@Quantity", product.Quantity));
                            cmd.Parameters.Add(new SqlParameter("@CustomerId", product.CustomerId));
                            cmd.Parameters.Add(new SqlParameter("@id", id));

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                */

        // DELETE: api/ApiWithActions/5
        // Test Failed, here are details:
        /* Test Name:	TestBangazonAPI.ProductsControllerTest.Test_Remove_Product
 Test FullName:	TestBangazonAPI.ProductsControllerTest.Test_Remove_Product
 Test Source:	C:\Users\grady\C29\backend\workspace\BangazonAPI\TestBangazonAPI\ProductsControllerTest.cs : line 198
 Test Outcome:	Failed
 Test Duration:	0:00:01.547

 Result StackTrace:	
 at TestBangazonAPI.ProductsControllerTest.Test_Remove_Product() in C:\Users\grady\C29\backend\workspace\BangazonAPI\TestBangazonAPI\ProductsControllerTest.cs:line 208
 --- End of stack trace from previous location where exception was thrown ---
 Result Message:	
 Assert.Equal() Failure
 Expected: NoContent
 Actual:   NotFound */


        
                [HttpDelete("{id}")]
                public async Task<IActionResult> Delete([FromRoute] int id)
                {
                    try
                    {
                        using (SqlConnection conn = Connection)
                        {
                            conn.Open();
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = @"DELETE FROM Product WHERE Id = @id";
                                cmd.Parameters.Add(new SqlParameter("@id", id));

                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                                }
                                throw new Exception("No rows affected");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        if (!ProductExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                private bool ProductExists(int id)
                {
                    using (SqlConnection conn = Connection)
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"SELECT Id, productTypeId, Price , Title, Description, Quantity
                                                FROM Product 
                                                WHERE Id = @id";
                            cmd.Parameters.Add(new SqlParameter("@id", id));

                            SqlDataReader reader = cmd.ExecuteReader();
                            return reader.Read();
                        }
                    }
                }
                


    }

}

