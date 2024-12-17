using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RegistrationApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RegistrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration([FromBody] Registration registration)
        {
            if (registration == null)
            {
                return BadRequest(new Response { StatusCode = 400, StatusMessage = "Invalid input" });
            }

            string connectionString = _configuration.GetConnectionString("ToysCon");

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Registration (UserName, Password, Email, IsActive) VALUES (@UserName, @Password, @Email, @IsActive)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserName", registration.UserName);
                        cmd.Parameters.AddWithValue("@Password", registration.Password);
                        cmd.Parameters.AddWithValue("@Email", registration.Email);
                        cmd.Parameters.AddWithValue("@IsActive", registration.IsActive);

                        await con.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            return Ok(new Response { StatusCode = 200, StatusMessage = "Data Inserted" });
                        }
                        else
                        {
                            return StatusCode(500, new Response { StatusCode = 500, StatusMessage = "Error inserting data" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { StatusCode = 500, StatusMessage = ex.Message });
            }

        }

        [HttpPost]
        [Route("login")]
        public string login(Registration registration)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ToysCon")))
                {
                    string query = "SELECT * FROM Registration WHERE Email = @Email AND Password = @Password ";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", registration.Email);
                        cmd.Parameters.AddWithValue("@Password", registration.Password);

                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    return "Valid User";
                }
                else
                {
                    return "Invalid User";
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                return "Error: " + ex.Message;
            }
        }

    }
}
