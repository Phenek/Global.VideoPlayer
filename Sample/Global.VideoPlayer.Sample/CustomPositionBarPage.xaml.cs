using System;
using Xamarin.Forms;

namespace Global.VideoPlayer.Sample
{
    public partial class CustomPositionBarPage : ContentPage
    {
        public CustomPositionBarPage()
        {
            InitializeComponent();
        }

        void OnPlayPauseButtonClicked(object sender, EventArgs args)
        {
            switch (videoPlayer.Status)
            {
                case VideoStatus.Playing:
                    videoPlayer.Pause();
                    break;
                case VideoStatus.Paused:
                    videoPlayer.Play();
                    break;
                case VideoStatus.NotReady:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnStopButtonClicked(object sender, EventArgs args)
        {
            videoPlayer.Stop();
        }
    }
}