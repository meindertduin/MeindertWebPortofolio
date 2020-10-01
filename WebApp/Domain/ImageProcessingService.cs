using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using WebApp.Models;

namespace WebApp.Domain
{
    public class ImageProcessingService : IImageProcessingService
    {
        public Task ChangeResolution(IFormFile file, int width, int height, string savePath)
        {
            using (var input = file.OpenReadStream())
            {
                using (var image = Image.Load(input))
                {
                    image.Mutate(x => 
                        x.Resize(new Size(width, height)));
                    
                    return image.SaveAsync(savePath, new JpegEncoder());
                }
            }
        }
    }
}