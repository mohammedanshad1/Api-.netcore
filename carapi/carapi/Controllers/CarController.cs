using carapi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace carapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {




        public readonly IConfiguration _configuration;
        public CarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllCars")]
        public string GetCars()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from car", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<car> carslist = new List<car>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    car cars = new car();
                    cars.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    cars.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    cars.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    carslist.Add(cars);
                }
            }
            if (carslist.Count > 0)
                return JsonConvert.SerializeObject(carslist);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }



        }


        [HttpPost]
        [Route("Insertcars")]
        public string InsertFood(int carId,string carName, string carType)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Insertcar", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@carId", carId);
                cmd.Parameters.AddWithValue("@carName", carName);
                cmd.Parameters.AddWithValue("@carType", carType);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                response.ErrorMessage = "cars inserted successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = "Internal server error: " + ex.Message;
            }
            return JsonConvert.SerializeObject(response);
        }

        [HttpPut]
        [Route("Putcars")]
        public string Putcar(string carName, string newcarName, string newcarType)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("UPDATE car SET Name = @newcarName, Type = @newcarType WHERE Name = @carName", conn);
                cmd.Parameters.AddWithValue("@carName", carName);
                cmd.Parameters.AddWithValue("@newcarName", newcarName);
                cmd.Parameters.AddWithValue("@newcarType", newcarType);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.ErrorMessage = "car updated successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No matching car item found";
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
        [Route("Deletecars")]
        public string Deletecar(string carName)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("DELETE FROM car WHERE Name = @carName", conn);
                cmd.Parameters.AddWithValue("@carName", carName);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.ErrorMessage = "car deleted successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No matching car item found";
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
