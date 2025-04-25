namespace RunGroopWebApp.Services.Services.interfaces
{
    public interface IFileService
    {
        Stream GetImageAsStream();
        byte[] GetImageByteArray();
    }
}
