using ImageSharp;
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
            using (FileStream stream = File.OpenRead(config.SourceFilePath))
            using (FileStream output = File.OpenWrite(config.SavePathFile))
            using (Image<Rgba32> image = Image.Load<Rgba32>(stream))
            {
                image.Resize(image.Width / 2, image.Height / 2).Save(output);
            }
            return Path.GetFileName(config.SavePathFile);
        }
    }
}
