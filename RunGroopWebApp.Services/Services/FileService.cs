
namespace RunGroopWebApp.Services.Services
{
   public  class FileService
    {
        private byte[] ConvertImageToByteArray()
        {
            var directory=Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, "Assets", "image.png");
            return File.ReadAllBytes(path);
        }
        public Stream GetImageAsStream()=>
            new MemoryStream(ConvertImageToByteArray());
        public byte[] GetImageByteArray() => ConvertImageToByteArray();
    }
}
