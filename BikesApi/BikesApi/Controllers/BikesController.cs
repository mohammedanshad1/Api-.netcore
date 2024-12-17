using BikesApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace BikesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        public BikesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetAllBikes")]
        public string GetBike()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from bikes", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Bikes> bikeslist = new List<Bikes>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Bikes bikeModels = new Bikes();
                    bikeModels.Name = Convert.ToString(dt.Rows[i]["name"]);
                    bikeModels.Type = Convert.ToString(dt.Rows[i]["type"]);
                    bikeModels.ImageUrl = Convert.ToString(dt.Rows[i]["imageUrl"]);  // Add this line

                    bikeslist.Add(bikeModels);
                }
            }
            if (bikeslist.Count > 0)
                return JsonConvert.SerializeObject(bikeslist);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }
        }
        [HttpPost]
        [Route("Insertbikes")]
        public string Insertbikes(string Name, string Type, string ImageUrl)  // Add ImageUrl parameter
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Bike", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bikeName", Name);
                cmd.Parameters.AddWithValue("@type", Type);
                cmd.Parameters.AddWithValue("@imageUrl", ImageUrl);  // Add this line
                cmd.Parameters.AddWithValue("@Critera", "INSERT");
                conn.Open();
                int val = cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                if (val >= 1)
                {
                    response.SuccessMessage = "Bike inserted successfully";
                }
                else
                {
                    response.ErrorMessage = "Something went wrong";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = "Internal server error: " + ex.Message;
            }
            return JsonConvert.SerializeObject(response);
        }

        [HttpPut]
        [Route("UpdateBike")]
        public string UpdateBike(string Name, string Type, string ImageUrl)  // Add ImageUrl parameter
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Bike", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bikeName", Name);
                cmd.Parameters.AddWithValue("@type", Type);
                cmd.Parameters.AddWithValue("@imageUrl", ImageUrl);  // Add this line
                cmd.Parameters.AddWithValue("@Critera", "UPDATE");
                conn.Open();
                int val = cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                if (val >= 1)
                {
                    response.SuccessMessage = "Bike updated successfully";
                }
                else
                {
                    response.ErrorMessage = "No bike found with the provided name";
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
        [Route("DeleteBike")]
        public string DeleteBike(string Name)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Bike", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bikeName",Name);
                cmd.Parameters.AddWithValue("@Critera", "DELETE");
                conn.Open();
                int val = cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                if (val >= 1)
                {
                    response.SuccessMessage = "Bikes  deleted successfully";
                }
                else
                {
                    response.ErrorMessage = "No Bikes found with the provided Name";
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
