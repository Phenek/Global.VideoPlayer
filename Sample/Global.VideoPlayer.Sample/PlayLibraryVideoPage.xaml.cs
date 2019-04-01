using System;
using Xamarin.Forms;

namespace Global.VideoPlayer.Sample
{
    public partial class PlayLibraryVideoPage : ContentPage
    {
        public PlayLibraryVideoPage()
        {
            InitializeComponent();
        }

        private async void OnShowVideoLibraryClicked(object sender, EventArgs args)
        {
            var btn = (Button) sender;
            btn.IsEnabled = false;

            var filename = await DependencyService.Get<IVideoPicker>().GetVideoFileAsync();

            if (!string.IsNullOrWhiteSpace(filename))
                videoPlayer.Source = new FileVideoSource
                {
                    File = filename
                };

            btn.IsEnabled = true;
        }
    }
}