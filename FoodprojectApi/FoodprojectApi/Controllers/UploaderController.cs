using FoodprojectApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace FoodprojectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploaderController : ControllerBase
    {
        [HttpPost]
        [Route("UploadFile")]
        public Response UploadFile([FromForm] FileModel fileModel)
        {
            Response response = new Response();
            try
            {
                string path = Path.Combine(@"D:\\MyImages", fileModel.FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    fileModel.file.CopyTo(stream);
                }
                response.StatusCode = 200;
                response.ErrorMessage = "Image created successfully with text name: " + fileModel.Text;

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.ErrorMessage = "Some error occured" + ex.Message; 
            }
            return response;

        }
        [HttpGet]
        [Route("GetImage")]
        public IActionResult GetImage(string fileName)
        {
            string path = Path.Combine(@"D:\\MyImages", fileName);
            if (System.IO.File.Exists(path))
            {
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }

    }
}
