using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApp.Domain
{
    public interface IImageProcessingService
    {
        public Task ChangeResolution(IFormFile file, int width, int height, string savePath);
    }
}