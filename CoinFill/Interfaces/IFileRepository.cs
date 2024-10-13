using CoinFill.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CoinFill.Interfaces
{
    public interface IFileRepository
    {
        string UserImagesPath { get; }
        string DefaultProfilePhotoPath { get; }
        string GetUserProfilePhotoPathOrDefaultPhotoPath(UserManager<CustomClient> userManager, IWebHostEnvironment environment, string userId);
        bool UserImageExists(IWebHostEnvironment environment, string imageName);
        Task<string> CreateUserImageAsync(IWebHostEnvironment environment, string imageNamePrefix, IFormFile file);
        bool DeleteUserImage(IWebHostEnvironment environment, string imageName);
    }
}
