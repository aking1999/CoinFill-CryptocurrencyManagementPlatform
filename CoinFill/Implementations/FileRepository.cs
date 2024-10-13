using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CoinFill.Implementations
{
    public class FileRepository : IFileRepository
    {
        private const string PROJECT = "CoinFill";
        private const string CLASS = "FileRepository";

        private const string CLIENT_DEFAULT = "client-default.png";
        public string DefaultProfilePhotoPath { get; }
        public string UserImagesPath { get; }
        public string LogsPath { get; }
        public string EmailLogsPath { get; }
        private ISystemErrorLogger _systemErrors { get; }

        public FileRepository()
        {
            UserImagesPath = @"/images/clients/profile-photos/";
            DefaultProfilePhotoPath = Path.Combine(UserImagesPath, CLIENT_DEFAULT);
            _systemErrors = new SystemErrorLogger();
        }

        public string GetUserProfilePhotoPathOrDefaultPhotoPath(UserManager<CustomClient> userManager, IWebHostEnvironment environment, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return DefaultProfilePhotoPath;

                var user = userManager.FindByIdAsync(userId).Result;

                if (user != null && UserImageExists(environment, user.ProfilePhoto))
                    return Path.Combine(UserImagesPath, user.ProfilePhoto);

                return DefaultProfilePhotoPath;
            }
            catch (Exception)
            {
                return DefaultProfilePhotoPath;
            }
        }

        public bool UserImageExists(IWebHostEnvironment environment, string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
                return false;

            return new FileInfo(Path.Combine(environment.WebRootPath, @"images/clients/profile-photos/" + imageName)).Exists;
        }

        public async Task<string> CreateUserImageAsync(IWebHostEnvironment environment, string imageNamePrefix, IFormFile file)
        {
            if (UserImageExists(environment, file.FileName))
                return file.FileName;

            string uniquieFileNameWithExtension = imageNamePrefix + "-" + Helper.GenerateNumbersId() +
                                      Path.GetExtension(file.FileName);

            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (uniquieFileNameWithExtension.Any(invalidFileNameChars.Contains))
            {
                foreach (var c in invalidFileNameChars)
                {
                    uniquieFileNameWithExtension = uniquieFileNameWithExtension.Replace(c, '-');
                }
            }

            var saveToPath = Path.Combine(environment.WebRootPath, @"/images/clients/profile-photos") + $@"/{uniquieFileNameWithExtension}";

            var invalidPathChars = Path.GetInvalidPathChars();
            if (saveToPath.Any(invalidPathChars.Contains))
            {
                foreach (var c in invalidPathChars)
                {
                    saveToPath = saveToPath.Replace(c, '-');
                }
            }

            using (var stream = File.Create(environment.ContentRootPath + @"/wwwroot" + saveToPath))
            {
                await file.CopyToAsync(stream);
                await stream.FlushAsync();
            }

            return uniquieFileNameWithExtension;
        }

        public bool DeleteUserImage(IWebHostEnvironment environment, string imageName)
        {
            if (UserImageExists(environment, imageName))
            {
                new FileInfo(Helper.CombinePaths(environment.WebRootPath, @"images/clients/profile-photos/" + imageName)).Delete();

                return true;
            }

            return false;
        }
    }
}
