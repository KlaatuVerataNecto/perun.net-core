using System.Threading;
using System.Threading.Tasks;

namespace infrastucture.libs.image
{
    
    public interface IImageService
    {

        void Resize(IImageConfig config);

        Task ResizeAsync(IImageConfig config,
            CancellationToken cancellationToken = new CancellationToken());

    }
    
}