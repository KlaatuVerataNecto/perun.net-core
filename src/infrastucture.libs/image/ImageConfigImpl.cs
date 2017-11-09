namespace infrastucture.libs.image
{

    internal class ImageConfigImpl : IImageConfig
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageConfigImpl"/> class.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="saveFilePath"></param>
        /// <param name="quality"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ImageConfigImpl(string sourceFilePath, string saveFilePath,
            int quality, int? width = null, int? height = null)
        {
            SourceFilePath = sourceFilePath;
            SaveFilePath = saveFilePath;
            Quality = quality;
            Width = width;
            Height = height;
        }

        #region # IImageConfig #

        /// <inheritdoc />
        public string SourceFilePath { get; }

        /// <inheritdoc />
        public string SaveFilePath { get; }

        /// <inheritdoc />
        public int Quality { get; }

        /// <inheritdoc />
        public int? Width { get; }

        /// <inheritdoc />
        public int? Height { get; }

        #endregion

    }

}