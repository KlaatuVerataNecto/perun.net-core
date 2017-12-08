using System.Threading;
using System.Threading.Tasks;

namespace infrastucture.libs.image
{
    
    public interface IImageService
    {
        void Resize(IImageConfig config);
        void Crop(IImageConfig config);
        Task CropAsync(IImageConfig config,
            CancellationToken cancellationToken = new CancellationToken());

    }
    
}