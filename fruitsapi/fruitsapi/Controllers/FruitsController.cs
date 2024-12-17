using fruitsapi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace fruitsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitsController : ControllerBase
    {


        public readonly IConfiguration _configuration;
        public FruitsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllfruits")]
        public string GetCars()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from fruits", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Fruits> list = new List<Fruits>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Fruits fruitss = new Fruits();

                    fruitss.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    fruitss.Address = Convert.ToString(dt.Rows[i]["Address"]);
                    list.Add(fruitss);
                }
            }
            if (list.Count > 0)
                return JsonConvert.SerializeObject(list);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }

        }




        [HttpPost]
        [Route("Insertfruits")]
        public string InsertFruits( string Name, string Address)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Insertfruits", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@fruitsName", Name);
                cmd.Parameters.AddWithValue("@fruitsAddress", Address);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                response.ErrorMessage = "fruits inserted successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = "Internal server error: " + ex.Message;
            }
            return JsonConvert.SerializeObject(response);
        }
          [HttpPut]
        [Route("Putfruits")]
        public string UpdateFruits(string Name, string newfruitsName, string newfruitsAddress)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("UPDATE fruits SET Name = @newfruitsName, Address = @newfruitsAddress WHERE Name = @fruitsName", conn);
                cmd.Parameters.AddWithValue("@fruitsName", Name);
                cmd.Parameters.AddWithValue("@newfruitsName", newfruitsName);
                cmd.Parameters.AddWithValue("@newfruitsAddress", newfruitsAddress);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.ErrorMessage = "fruits updated successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No matching fruits item found";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = "Internal server error: " + ex.Message;
            }
            return JsonConvert.SerializeObject(response);
        }


        [HttpDelete]
        [Route("Deletefruits")]
        public string Deletefruits(string fruitsName)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("DELETE FROM fruits WHERE Name = @fruitsName", conn);
                cmd.Parameters.AddWithValue("@fruitsName", fruitsName);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.ErrorMessage = "fruits deleted successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No matching cfruits item found";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = "Internal server error: " + ex.Message;
            }
            return JsonConvert.SerializeObject(response);
        }
    }
}