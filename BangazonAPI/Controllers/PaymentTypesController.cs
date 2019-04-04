using System;
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
    public class PaymentTypesController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public PaymentTypesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: api/PaymentTypes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select Id, AcctNumber, [Name], CustomerId
                                        From PaymentType";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<PaymentType> PaymentTypes = new List<PaymentType>();

                    while (reader.Read())
                    {
                        PaymentType paymentType = new PaymentType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            AcctNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                        };

                        PaymentTypes.Add(paymentType);
                    }
                    reader.Close();
                    return Ok(PaymentTypes);
                }
            }
        }
        // GET: api/PaymentTypes/5
        [HttpGet("{id}", Name = "GetPaymentType")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select Id, AcctNumber, [Name], CustomerId
                                        From PaymentType
                                        Where Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    PaymentType paymentType = null;

                    if (reader.Read())
                    {
                        paymentType = new PaymentType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            AcctNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                        };
                    }
                    reader.Close();
                    return Ok(paymentType);
                }
            }
        }

        // POST: api/PaymentTypes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentType paymentType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PaymentType (AcctNumber, Name, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@AcctNumber, @Name, @CustomerId)";
                    cmd.Parameters.Add(new SqlParameter("@AcctNumber", paymentType.AcctNumber));
                    cmd.Parameters.Add(new SqlParameter("@Name", paymentType.Name));
                    cmd.Parameters.Add(new SqlParameter("@CustomerId", paymentType.CustomerId));

                    int newId = (int)cmd.ExecuteScalar();
                    paymentType.Id = newId;
                    return CreatedAtRoute("GetPaymentType", new { id = newId }, paymentType);
                }
            }
        }

        // PUT: api/PaymentTypes/5
        [HttpPut("{id}")]
        public async Task <IActionResult> Put([FromRoute] int id, [FromBody] PaymentType paymentType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE PaymentType
                                            SET AcctNumber = @AcctNumber, Name = @Name, CustomerId = @CustomerId
                                            WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@AcctNumber", paymentType.AcctNumber));
                    cmd.Parameters.Add(new SqlParameter("@Name", paymentType.Name));
                    cmd.Parameters.Add(new SqlParameter("@CustomerId", paymentType.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                    return NoContent();

                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM PaymentType WHERE id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
