using SkiaSharp;
using System;
using System.IO;

namespace infrastucture.libs.image
{
    public class ImageConfig
    {
        private string _sourcefilePath;
        private string _savePathFile;
        // TODO:
        //private int _quality;
        //private int _width;
        //private int _heigh;

        public string SourceFilePath { get => _sourcefilePath; }
        public string SavePathFile { get => _savePathFile; }

        public ImageConfig CreateConfigFromImageFile(string sourcefilePath)
        {
            this._sourcefilePath = sourcefilePath;
            return this;
        }
        public ImageConfig WithSaveTo(string savePathFile)
        {
            this._savePathFile = savePathFile;
            return this;
        }
    }

    public static class ImageService 
    {
        // TODO: quality, resize params
        public static string ResizeAndSave(ImageConfig config)
        {
            using (var input = File.OpenRead(config.SourceFilePath))
            {
                using (var inputStream = new SKManagedStream(input))
                {
                    using (var original = SKBitmap.Decode(inputStream))
                    {
                        using (var resized = original.Resize(new SKImageInfo(100, 100), SKBitmapResizeMethod.Lanczos3))
                        {
                            if (resized == null) return null;

                            using (var image = SKImage.FromBitmap(resized))
                            {
                                using (var output = File.OpenWrite(config.SavePathFile))
                                {
                                    image.Encode(SKEncodedImageFormat.Jpeg, 90).SaveTo(output);
                                }
                            }
                        }
                    }
                }
            }

            return Path.GetFileName(config.SavePathFile);
        }
    }
}
