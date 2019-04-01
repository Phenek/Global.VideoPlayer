using System;
using Xamarin.Forms;

namespace Global.VideoPlayer.Sample
{
    public partial class CustomTransportPage : ContentPage
    {
        public CustomTransportPage()
        {
            InitializeComponent();
        }

        private void OnPlayPauseButtonClicked(object sender, EventArgs args)
        {
            if (videoPlayer.Status == VideoStatus.Playing)
                videoPlayer.Pause();
            else if (videoPlayer.Status == VideoStatus.Paused) videoPlayer.Play();
        }

        private void OnStopButtonClicked(object sender, EventArgs args)
        {
            videoPlayer.Stop();
        }
    }
}