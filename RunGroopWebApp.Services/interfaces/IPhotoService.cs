using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace RunGroop.Data.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicUrl);
    }
}