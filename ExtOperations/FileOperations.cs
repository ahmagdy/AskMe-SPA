using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Aspcorespa.ExtOperations
{
    public static  class FileOperations
    {
    
        public static async Task<string> SaveImageAsync(IFormFile image, IHostingEnvironment environment)
        {
            var folderName = Path.Combine(environment.WebRootPath, "uploads");
            var extension = Path.GetExtension(image.FileName);
            var imgPath = Path.Combine(folderName, $"{Path.GetRandomFileName()}{extension}");
            using (var fileStream = new FileStream(imgPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return imgPath;
        }

        public static void DeleteImage(string imagePath)
        {
            File.Delete(imagePath);
        }
    }
}
