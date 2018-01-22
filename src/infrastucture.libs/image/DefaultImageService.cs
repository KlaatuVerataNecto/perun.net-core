#region # using statements #

using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.Primitives;

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

        public string getImageExtensionByContentType(string contentType)
        {
            switch(contentType)
            {
                case "image/jpeg": return ".jpg";
                case "image/gif": return ".gif";
                case "image/png": return ".png"; 
                default: return "jpg";
            }
        }

        #region # IImageService #

        /// <inheritdoc />
        public void Resize(IImageConfig config)
        {
            if (!config.MaxWidth.HasValue || config.MaxWidth.Value <= 0)
                return;

            //if (!config.MaxHeight.HasValue || config.MaxHeight.Value <= 0)
            //    return;

            using (var original = Image.Load(config.SourceFilePath))
            {
                using (var image =new Image<Rgba32>(original.Width, original.Height))
                {
                    var maxWidth = config.MaxWidth.Value;
                    //var maxHeight = config.MaxHeight.Value;
                    var newWidth = original.Width;
                    var newHeight = original.Height;

                    if (original.Width > config.MaxWidth.Value)
                    {

                        //var ratioY = (double)maxHeight / original.Height;
                        //var ratio = Math.Min(ratioX, ratioY);

                        //newWidth = (int)(original.Width * ratio);
                        //newHeight = (int)(original.Height * ratio);
                        newWidth = maxWidth;
                        var ratioX = (double)maxWidth / original.Width;
                        newHeight = (int)(original.Height * ratioX);
                    }

                    image.Mutate(x => x.Fill(Rgba32.Black));
                    image.Mutate(x => x.DrawImage(original, 1f, Size.Empty, Point.Empty));
                    image.Mutate(x => x.Resize(newWidth, newHeight));
                    
                    image.Save(config.SaveFilePath, new JpegEncoder {Quality = config.Quality});
                }
            }
        }

        public void Crop(IImageConfig config)
        {
            if (!config.Width.HasValue || config.Width.Value <= 0 ||
                !config.Height.HasValue || config.Height.Value <= 0)
                return;

            using (var original = Image.Load(config.SourceFilePath))
            {
                using (var image =
                    new Image<Rgba32>(original.Width, original.Height))
                {
                    image.Mutate(x => x.Fill(Rgba32.White));
                    image.Mutate(x => x.DrawImage(original, 1f, Size.Empty, Point.Empty));
                    image.Mutate(x => x.Crop(new Rectangle(config.X ?? 0,
                                                config.Y ?? 0,
                                                config.Width.Value, config.Height.Value)
                                            )
                    );

                    image.Save(config.SaveFilePath, new JpegEncoder { Quality = config.Quality });
                }
            }
        }

        /// <inheritdoc />
        public Task CropAsync(IImageConfig config, CancellationToken cancellationToken = new CancellationToken())
        {
            Crop(config);
            return Task.FromResult(0);
        }

        #endregion

    }

}