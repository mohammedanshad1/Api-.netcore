using foodapi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace foodapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public FoodController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public string GetUsers()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from food", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Food> foodlist = new List<Food>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Food food = new Food();
                    food.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    food.Address = Convert.ToString(dt.Rows[i]["Address"]);
                    foodlist.Add(food);
                }
            }
            if (foodlist.Count > 0)
                return JsonConvert.SerializeObject(foodlist);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("InsertFood")]
        public string InsertFood(string foodName, string foodAddress)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("InsertFood", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@foodName", foodName);
                cmd.Parameters.AddWithValue("@foodAddress", foodAddress);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                response.ErrorMessage = "Food inserted successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ErrorMessage = "Internal server error: " + ex.Message;
            }
            return JsonConvert.SerializeObject(response);
        }

        [HttpPut]
        [Route("PutFood")]
        public string PutFood(string foodName, string newFoodName, string newFoodAddress)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("UPDATE food SET Name = @newFoodName, Address = @newFoodAddress WHERE Name = @foodName", conn);
                cmd.Parameters.AddWithValue("@foodName", foodName);
                cmd.Parameters.AddWithValue("@newFoodName", newFoodName);
                cmd.Parameters.AddWithValue("@newFoodAddress", newFoodAddress);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.ErrorMessage = "Food updated successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No matching food item found";
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
        [Route("DeleteFood")]
        public string DeleteFood(string foodName)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("DELETE FROM food WHERE Name = @foodName", conn);
                cmd.Parameters.AddWithValue("@foodName", foodName);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.ErrorMessage = "Food deleted successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.ErrorMessage = "No matching food item found";
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
