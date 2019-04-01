using System;
using System.ComponentModel;
using System.IO;
using Android.Content;
using Android.Media;
using Android.Views;
using Android.Widget;
using Global.VideoPlayer;
using Global.VideoPlayer.Droid;
using Xamarin.Forms;
using Uri = Android.Net.Uri;

[assembly: ExportRenderer(typeof(VideoPlayer),
    typeof(VideoPlayerRenderer))]

namespace Global.VideoPlayer.Droid
{
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, FrameLayout>
    {
        private bool _isPrepared;
        private MediaController _mediaController; // Used to display transport controls
        private int _videoHeight;
        private VideoView _videoView;
        private int _videoWidth;

        public VideoPlayerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {
                    // Save the VideoView for future reference
                    _videoView = new VideoView(Context);

                    // Put the VideoView in a RelativeLayout
                    var frameLayout = new FrameLayout(Context);
                    frameLayout.AddView(_videoView);
                    frameLayout.Visibility = ViewStates.Invisible;

                    // Handle a VideoView event
                    _videoView.Prepared += OnVideoViewPrepared;

                    SetNativeControl(frameLayout);
                }

                SetAreTransportControlsEnabled();
                SetSource();

                args.NewElement.UpdateStatus += OnUpdateStatus;
                args.NewElement.PlayRequested += OnPlayRequested;
                args.NewElement.PauseRequested += OnPauseRequested;
                args.NewElement.StopRequested += OnStopRequested;
            }

            if (args.OldElement != null)
            {
                args.OldElement.UpdateStatus -= OnUpdateStatus;
                args.OldElement.PlayRequested -= OnPlayRequested;
                args.OldElement.PauseRequested -= OnPauseRequested;
                args.OldElement.StopRequested -= OnStopRequested;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null && _videoView != null) _videoView.Prepared -= OnVideoViewPrepared;
            if (Element != null) Element.UpdateStatus -= OnUpdateStatus;
            base.Dispose(disposing);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            UpdateVideoSize();
        }

        private void UpdateVideoSize()
        {
            if (Element.Aspect == VideoAspect.Fill)
            {
                _videoView.Layout(0, 0, Width, Height);
                return;
            }

            // assume video size = view size if the player has not been loaded yet
            var vWidth = _videoWidth > 0 ? _videoWidth : Width;
            var vHeight = _videoHeight > 0 ? _videoHeight : Height;

            if (_videoWidth < 0 && _videoHeight < 0)
                return;

            var scaleWidth = Width / (double) vWidth;
            var scaleHeight = Height / (double) vHeight;

            double scale = 1;

            switch (Element.Aspect)
            {
                case VideoAspect.AspectFit:
                    scale = Math.Min(scaleWidth, scaleHeight);
                    break;
                case VideoAspect.AspectFill:
                    scale = Math.Max(scaleWidth, scaleHeight);
                    break;
            }

            var scaledWidth = (int) Math.Round(vWidth * scale);
            var scaledHeight = (int) Math.Round(vHeight * scale);

            // center the video
            var l = (Width - scaledWidth) / 2;
            var t = (Height - scaledHeight) / 2;
            var r = l + scaledWidth;
            var b = t + scaledHeight;
            _videoView.Layout(l, t, r, b);
        }

        private void OnVideoViewPrepared(object sender, EventArgs args)
        {
            if (sender is MediaPlayer mp)
            {
                Control.Visibility = ViewStates.Visible;
                mp.Looping = Element.Loop;
                _isPrepared = true;
                ((IVideoPlayerController) Element).Duration = TimeSpan.FromMilliseconds(_videoView.Duration);

                mp.VideoSizeChanged += (s, a) =>
                {
                    _videoWidth = mp.VideoWidth;
                    _videoHeight = mp.VideoHeight;
                    UpdateVideoSize();
                };
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == VideoPlayer.NativeControlsProperty.PropertyName)
                SetAreTransportControlsEnabled();
            else if (args.PropertyName == VideoPlayer.SourceProperty.PropertyName)
                SetSource();
            else if (args.PropertyName == VideoPlayer.PositionProperty.PropertyName)
                if (Math.Abs(_videoView.CurrentPosition - Element.Position.TotalMilliseconds) > 1000)
                    _videoView.SeekTo((int) Element.Position.TotalMilliseconds);
        }

        private void SetAreTransportControlsEnabled()
        {
            if (Element.NativeControls)
            {
                _mediaController = new MediaController(Context);
                _mediaController.SetMediaPlayer(_videoView);
                _videoView.SetMediaController(_mediaController);
            }
            else
            {
                _videoView.SetMediaController(null);

                if (_mediaController != null)
                {
                    _mediaController.SetMediaPlayer(null);
                    _mediaController = null;
                }
            }
        }

        private void SetSource()
        {
            _isPrepared = false;
            var hasSetSource = false;

            if (Element.Source is UriVideoSource)
            {
                var uri = (Element.Source as UriVideoSource).Uri;

                if (!string.IsNullOrWhiteSpace(uri))
                {
                    _videoView.SetVideoURI(Uri.Parse(uri));
                    hasSetSource = true;
                }
            }
            else if (Element.Source is FileVideoSource)
            {
                var filename = (Element.Source as FileVideoSource).File;

                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _videoView.SetVideoPath(filename);
                    hasSetSource = true;
                }
            }
            else if (Element.Source is ResourceVideoSource)
            {
                var package = Context.PackageName;
                var path = (Element.Source as ResourceVideoSource).Path;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    var filename = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                    var uri = "android.resource://" + package + "/raw/" + filename;
                    _videoView.SetVideoURI(Uri.Parse(uri));
                    hasSetSource = true;
                }
            }

            if (hasSetSource && Element.AutoPlay) _videoView.Start();
        }

        // Event handler to update status
        private void OnUpdateStatus(object sender, EventArgs args)
        {
            var status = VideoStatus.NotReady;

            if (_isPrepared) status = _videoView.IsPlaying ? VideoStatus.Playing : VideoStatus.Paused;

            ((IVideoPlayerController) Element).Status = status;

            // Set Position property
            var timeSpan = TimeSpan.FromMilliseconds(_videoView.CurrentPosition);
            ((IElementController) Element).SetValueFromRenderer(VideoPlayer.PositionProperty, timeSpan);
        }

        // Event handlers to implement methods
        private void OnPlayRequested(object sender, EventArgs args)
        {
            _videoView.Start();
        }

        private void OnPauseRequested(object sender, EventArgs args)
        {
            _videoView.Pause();
        }

        private void OnStopRequested(object sender, EventArgs args)
        {
            Control.Visibility = ViewStates.Invisible;
            _videoView.StopPlayback();
        }
    }
}