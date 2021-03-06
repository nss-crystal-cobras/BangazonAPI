﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomersController(IConfiguration config)
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

        //If the query string parameter of? _include = products is provided, then any products that the customer is selling should be included in the response.

        //If the query string parameter of? _include = payments is provided, then any payment types that the customer has used to pay for an order should be included in the response.

        //If the query string parameter of q is provided when querying the list of customers, then any customer that has property value that matches the pattern should be returned.

        // GET: api/Customers
        /*
        [HttpGet]
        public IEnumerable<IActionResult> Get(string include, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    
                        if ( include == "products")
                        {
                       
                        }
                        else if ( include == "payments")
                        {
                            cmd.CommandText = @"SELECT FirstName, LastName, AcctNumber, Name as PaymentName
                                            FROM Customer c
                                            LEFT JOIN PaymentType t
                                            ON c.id = t.CustomerId
                                            WHERE 1 = 1";

                        }
                        else
                        {
                            cmd.CommandText = @"SELECT Id, FirstName, LastName
                                            FROM Customer";
                        }
                        if(!string.IsNullOrWhiteSpace(q))
                        {
                        cmd.CommandText += @" AND
                                            (c.FirstName LIKE @q OR 
                                             c.LastName LIKE @q)";
                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                        }

                            
                    SqlDataReader reader = cmd.ExecuteReader();

                    //new dictionary to store the query return of all customer objects

                    Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
                    while (reader.Read())
                    {
                            Customer customer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                            
                        };
                        PaymentType paymentType = new PaymentType()
                        {

                            Name = reader.GetString(reader.GetOrdinal("PaymentName")),
                            AcctNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber"))

                        };
                            
                            
                        //add the individual instance of each customer above to the dictionary
                        customers.Add(customer.Id, customer);
                    }
                    reader.Close();
                    return customers.Values.ToList();
                }
            }
        }
        */
        // GET: api/Customers/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get([FromRoute] int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, FirstName, LastName
                        FROM Customer
                        WHERE Id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Customer customer = null;

                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName"))
                        };
                    }
                    reader.Close();

                    return Ok(customer);
                }
            }
        }

        // POST:
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Customer (FirstName, LastName)
                                        OUTPUT INSERTED.Id
                                        VALUES (@FirstName, @LastName)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));


                    int newId = (int)cmd.ExecuteScalar();
                    customer.Id = newId;

                    return CreatedAtRoute("GetCustomer", new { id = newId }, customer);

                }
            }
        }

        // PUT: 
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Customer customer)
        {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Customer
                                            SET FirstName = @FirstName, 
                                                LastName = @LastName
                                            WHERE Id = @Id";
                        cmd.Parameters.Add(new SqlParameter("@FirstName", customer.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", customer.LastName));
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        cmd.ExecuteNonQuery();
                        return NoContent();

                    }
                }
            }
        
        // DELETE: 
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Customer WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
