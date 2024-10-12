
namespace RunGroopWebApp.Services.interfaces
{
    public interface IFileService
    {
        Stream GetImageAsStream();
        byte[] GetImageByteArray();
    }
}
