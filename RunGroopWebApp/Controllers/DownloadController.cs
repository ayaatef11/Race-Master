using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Services.interfaces;
using System.IO;

namespace RunGroopWebApp.Controllers
{
    public class DownloadController(IFileService fileService) : Controller
    {

        private const string MimeType = "img/png";
        private const string FileName = "image.png";
        [HttpGet]
        public IActionResult Get()
        {
            return View();
        }

        [HttpPost]
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
        [HttpGet("byte")]
        public IActionResult GetImageASByteArray()
        {
            var image = fileService.GetImageByteArray();
            return File(image, MimeType, FileName);
        }
        [HttpGet("stream")]
        public IActionResult GetImageASStream()
        {
            var image = fileService.GetImageAsStream();
            return File(image, MimeType, FileName);
        }
       
     
    }
   
}
