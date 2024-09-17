using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Services.interfaces;

namespace RunGroopWebApp.Controllers
{
    public class DownloadsController(IFileService fileService) : Controller
    {
        private const string MimeType = "img/png";
        private const string FileName = "image.png";
        [HttpGet("byte")]
        public IActionResult GetImageASByteArray()
        {
            var image=fileService.GetImageByteArray();
            return File(image,MimeType,FileName);
        }
        [HttpGet("stream")]
        public IActionResult GetImageASStream() { 
            var image=fileService.GetImageAsStream();
            return File(image,MimeType,FileName);
        }
    }
}
