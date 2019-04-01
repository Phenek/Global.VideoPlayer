using Xamarin.Forms;

namespace Global.VideoPlayer
{
    public class ResourceVideoSource : VideoSource
    {
        public static readonly BindableProperty PathProperty =
            BindableProperty.Create(nameof(Path), typeof(string), typeof(ResourceVideoSource));

        public string Path
        {
            set => SetValue(PathProperty, value);
            get => (string) GetValue(PathProperty);
        }
    }
}