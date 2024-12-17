using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sampleApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace sampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string connectionString;

        public ProductsController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? throw new ArgumentNullException("ConnectionStrings:SqlServerDb");
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO products (Name, Age) VALUES (@Name, @Age)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productDto.Name);
                        command.Parameters.AddWithValue("@Age", productDto.Age);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM products";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Age = reader.GetInt32(2)
                                };

                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            if (products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }
    }
}
