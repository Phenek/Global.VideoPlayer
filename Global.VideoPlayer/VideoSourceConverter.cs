using System;
using Xamarin.Forms;

namespace Global.VideoPlayer
{
    public class VideoSourceConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException("Cannot convert null or whitespace to ImageSource");
            return Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme != "file"
                ? VideoSource.FromUri(value)
                : VideoSource.FromResource(value);

        }
    }
}