using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sportsapi.Model;
using System.Data;
using System.Data.SqlClient;

namespace sportsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public  SportsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllUsers")]
          public string GetUsers()
          {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from games", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Sports> sportslist = new List<Sports>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Sports sport = new Sports();
                    sport.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    sport.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    sportslist.Add(sport);
                }
            }
            if (sportslist.Count > 0)
                return JsonConvert.SerializeObject(sportslist);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }


        }

        [HttpPost]
        [Route("Insertsports")]
        public string InsertFood(string sportsName, string sportsType)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Insertsports", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sportsName", sportsName);
                cmd.Parameters.AddWithValue("@sportsType", sportsType);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                response.ErrorMessage = "sports inserted successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = "Internal server error: " + ex.Message;
            }
            return JsonConvert.SerializeObject(response);
        }

        [HttpPut]
        [Route("Putsports")]
        public string PutFood(string sportsName, string newsportsName, string newsportsType)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("UPDATE games SET Name = @newsportsName, Type = @newsportsType WHERE Name = @sportsName", conn);
                cmd.Parameters.AddWithValue("@sportsName", sportsName);
                cmd.Parameters.AddWithValue("@newsportsName", newsportsName);
                cmd.Parameters.AddWithValue("@newsportsType", newsportsType);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.ErrorMessage = "sports updated successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No matching sports item found";
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
        [Route("Deletesports")]
        public string Deletesports(string sportsName)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("DELETE FROM games WHERE Name = @sportsName", conn);
                cmd.Parameters.AddWithValue("@sportsName", sportsName);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.ErrorMessage = "sports deleted successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No matching sports item found";
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
