namespace Skype2.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Shared.Config;

    [ValueConversion(typeof(long), typeof(ImageSource))]
    internal class ImageIdToImageSourceConverter : IValueConverter
    {
        private readonly Dictionary<long, ImageSource> _imageCache = new Dictionary<long, ImageSource>();

        public static ImageIdToImageSourceConverter Default { get; } = new ImageIdToImageSourceConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long imageId = (long)value;

            if (_imageCache.TryGetValue(imageId, out ImageSource cachedImage))
            {
                return cachedImage;
            }

            ImageSource imageSource = new BitmapImage(new Uri($"http://{Constants.ServerIp}:{Constants.HttpPort}/images/get/{imageId}"));

            _imageCache[imageId] = imageSource;

            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"ConvertBack is not supported on {nameof(ImageIdToImageSourceConverter)}.");
        }
    }
}