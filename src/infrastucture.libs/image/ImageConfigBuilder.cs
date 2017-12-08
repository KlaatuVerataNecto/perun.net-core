#region # using statements #

using System;
using infrastructure.libs.validators;

#endregion

namespace infrastucture.libs.image
{

    public class ImageConfigBuilder
    {

        #region # Variables #

        private string _sourceFilePath;
        private string _saveFilePath;
        private int _quality;
        private int? _x;
        private int? _y;
        private int? _width;
        private int? _height;
        private int? _maxWidth;
        private int? _maxHeight;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageConfigBuilder"/>
        /// class.
        /// </summary>
        public ImageConfigBuilder()
        {
        }

        #region # Methods #

        #region == Public ==

        public ImageConfigBuilder WithSourceFilePath(string value)
        {
            CustomValidators.StringNotNullorEmpty(value, null);
            CustomValidators.IsValidFilePath(value, nameof(value));

            _sourceFilePath = value;
            return this;
        }

        public ImageConfigBuilder WithSaveFilePath(string value)
        {
            CustomValidators.StringNotNullorEmpty(value, null);
            CustomValidators.IsValidFilePath(value, nameof(value));

            _saveFilePath = value;
            return this;
        }

        public ImageConfigBuilder WithQuality(int value)
        {
            if (value < 50 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "The quality range must be between 50 and 100");
            }

            _quality = value;
            return this;
        }

        public ImageConfigBuilder WithX(int value)
        {
            CustomValidators.IntNotNegative(value, null);

            _x = value;
            return this;
        }

        public ImageConfigBuilder WithY(int value)
        {
            CustomValidators.IntNotNegative(value, null);

            _y = value;
            return this;
        }

        public ImageConfigBuilder WithWidth(int value)
        {
            CustomValidators.IntNotNegative(value, null);

            _width = value;
            return this;
        }

        public ImageConfigBuilder WithHeight(int value)
        {
            CustomValidators.IntNotNegative(value, null);

            _height = value;
            return this;
        }

        public ImageConfigBuilder WithMaxWidth(int value)
        {
            CustomValidators.IntNotNegative(value, null);

            _maxWidth = value;
            return this;
        }

        public ImageConfigBuilder WithMaxHeight(int value)
        {
            CustomValidators.IntNotNegative(value, null);

            _maxHeight = value;
            return this;
        }

        public IImageConfig Build() => new ImageConfigImpl(_sourceFilePath,
            _saveFilePath, _quality, _x, _y, _width, _height, _maxWidth, _maxHeight);

        #endregion

        #endregion

    }

}