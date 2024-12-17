using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayersApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace PlayersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public PlayersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetAllPlayers")]
        public string GetPlayers()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from players", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Players> playerslist = new List<Players>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Players playerModels = new Players();
                    playerModels.footballerName = Convert.ToString(dt.Rows[i]["footballerName"]);
                    playerModels.age = Convert.ToInt32(dt.Rows[i]["age"]);
                    playerModels.postion = Convert.ToString(dt.Rows[i]["postion"]);
                    playerModels.team = Convert.ToString(dt.Rows[i]["team"]);
                    playerModels.seasonGoals = Convert.ToInt32(dt.Rows[i]["seasonGoals"]);

                    playerslist.Add(playerModels);
                }
            }
            if (playerslist.Count > 0)
                return JsonConvert.SerializeObject(playerslist);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }


        }
        [HttpPost]
        [Route("InsertPlayers")]
        public string Insertfootballer(string footballerName, int age, string postion, string team, int seasonGoals)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Football", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@footballerName", footballerName);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@postion", postion);
                cmd.Parameters.AddWithValue("@team", team);
                cmd.Parameters.AddWithValue("@seasonGoals", seasonGoals);
                cmd.Parameters.AddWithValue("@Critera", "INSERT");
                conn.Open();
                int val = cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                if (val >= 1)
                {
                    response.SuccessMessage = "player inserted successfully";

                }
                else
                {
                    response.ErrorMessage = "Something Went Wrong";
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
        [Route("UpdatePlayer")]
        public string UpdatePlayer(int id, string footballerName, int age, string postion, string team, int seasonGoals)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Football", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@footballerName", footballerName);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@postion", postion);
                cmd.Parameters.AddWithValue("@team", team);
                cmd.Parameters.AddWithValue("@seasonGoals", seasonGoals);
                cmd.Parameters.AddWithValue("@Critera", "UPDATE");
                conn.Open();
                int val = cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                if (val >= 1)
                {
                    response.SuccessMessage = "Player updated successfully";
                }
                else
                {
                    response.ErrorMessage = "No Player found with the provided ID";
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
        [Route("DeletePlayer")]
        public string DeletePlayer(int id)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Football", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Critera", "DELETE");
                conn.Open();
                int val = cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                if (val >= 1)
                {
                    response.SuccessMessage ="Players  deleted successfully";
                }
                else
                {
                    response.ErrorMessage = "No Players found with the provided ID";
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
