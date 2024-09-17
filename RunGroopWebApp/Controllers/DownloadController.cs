using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace RunGroopWebApp.Controllers
{
    [Route("api/[controller]")]
    public class DownloadController : Controller
    {
        [HttpPost("FileUpload")]
        public async Task< IActionResult> Get(List<IFormFile> files)
        {
            var size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {

                if (formFile.Length > 0)
                {

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), formFile.FileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))

                        await formFile.CopyToAsync(stream);

                }
            }
                return Ok(new { files.Count, size, filePaths });
            
        }
        public IActionResult Get()
        {
            return View();
        }
    }
   
}
