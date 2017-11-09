#region # using statements #

using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

#endregion

namespace infrastucture.libs.image
{

    public class DefaultImageService : IImageService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultImageService"/>
        /// class.
        /// </summary>
        public DefaultImageService()
        {
        }

        #region # IImageService #

        /// <inheritdoc />
        public void Resize(IImageConfig config)
        {
            if (!config.Width.HasValue || !config.Height.HasValue)
                return;

            using (var image = Image.Load(config.SourceFilePath))
            {
                image.Mutate(x => x.Resize(config.Width.Value, config.Height.Value));
                image.Save(config.SaveFilePath, new JpegEncoder());
            }
        }

        /// <inheritdoc />
        public Task ResizeAsync(IImageConfig config,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Resize(config);
            return Task.FromResult(0);
        }

        #endregion

    }

}