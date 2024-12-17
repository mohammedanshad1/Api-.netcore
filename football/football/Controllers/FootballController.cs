using football.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace football.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FootballController : ControllerBase
    {


        public readonly IConfiguration _configuration;
        public FootballController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllPlayer")]
        public string GetPlayers()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from football", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<footballs> footballslist = new List<footballs>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    footballs footballer = new footballs();
                    footballer.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    footballer.Team = Convert.ToString(dt.Rows[i]["Team"]);
                    footballer.Posiiton = Convert.ToString(dt.Rows[i]["Posiiton"]);
                    footballslist.Add(footballer);
                }
            }
            if (footballslist.Count > 0)
                return JsonConvert.SerializeObject(footballslist);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }



        }


        [HttpPost]
        [Route("Insertplayers")]
        public string Insertplayers( string name, string team,string posiiton)
        {
            Response response = new Response();
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
                SqlCommand cmd = new SqlCommand("Insertplayers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.AddWithValue("@playersId", Id);
                cmd.Parameters.AddWithValue(" @playersName", name);
                cmd.Parameters.AddWithValue("@playersTeam",team);
                cmd.Parameters.AddWithValue("@playersPosition", posiiton);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                response.StatusCode = 200;
                response.ErrorMessage = "players inserted successfully";
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
