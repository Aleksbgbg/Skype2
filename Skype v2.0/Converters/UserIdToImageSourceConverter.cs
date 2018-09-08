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
    internal class UserIdToImageSourceConverter : IValueConverter
    {
        private readonly Dictionary<long, ImageSource> _imageCache = new Dictionary<long, ImageSource>();

        public static UserIdToImageSourceConverter Default { get; } = new UserIdToImageSourceConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long userId = (long)value;

            if (_imageCache.TryGetValue(userId, out ImageSource cachedImage))
            {
                return cachedImage;
            }

            ImageSource imageSource = new BitmapImage(new Uri($"{Constants.HttpServerAddress}/user/{userId}/get/image"));

            _imageCache[userId] = imageSource;

            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"ConvertBack is not supported on {nameof(UserIdToImageSourceConverter)}.");
        }
    }
}