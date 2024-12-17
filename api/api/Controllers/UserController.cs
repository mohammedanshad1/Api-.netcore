using api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public string GetUsers()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Userappconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * from demodb", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<User> userlist = new List<User>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    User user = new User();
                  
                    user.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    user.Age = Convert.ToString(dt.Rows[i]["Age"]);
                   
                    userlist.Add(user);

                }

            }
            if (userlist.Count > 0)
                return JsonConvert.SerializeObject(userlist);
            else
            {

                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                return JsonConvert.SerializeObject(response);
            }
        }
        




    }
}
