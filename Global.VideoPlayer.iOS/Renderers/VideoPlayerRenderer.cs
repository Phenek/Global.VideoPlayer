﻿using System;
using System.ComponentModel;
using System.IO;
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using Global.VideoPlayer.iOS.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(VideoPlayer),
    typeof(VideoPlayerRenderer))]

namespace Global.VideoPlayer.iOS.iOS
{
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, UIView>
    {
        private AVPlayerViewController _playerViewController; // solely for ViewController property
        private AVPlayer _player;
        private AVPlayerItem _playerItem;
        private NSObject _videoEndNotificationToken;

        public override UIViewController ViewController => _playerViewController;

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {
                    // Create AVPlayerViewController
                    _playerViewController = new AVPlayerViewController();

                    // Set Player property to AVPlayer
                    _player = new AVPlayer();
                    _playerViewController.Player = _player;
                    _playerViewController.VideoGravity = (AVLayerVideoGravity) Element.Aspect;
                    _player.Muted = true;

                    var x = _playerViewController.View;

                    // Use the View from the controller as the native control
                    SetNativeControl(_playerViewController.View);
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
            base.Dispose(disposing);

            if (_player != null) _player.ReplaceCurrentItemWithPlayerItem(null);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == VideoPlayer.TimeToEndProperty.PropertyName)
                return;

            if (args.PropertyName == VideoPlayer.NativeControlsProperty.PropertyName)
            {
                SetAreTransportControlsEnabled();
            }
            else if (args.PropertyName == VideoPlayer.SourceProperty.PropertyName)
            {
                SetSource();
            }
            else if (args.PropertyName == VideoPlayer.PositionProperty.PropertyName)
            {
                var controlPosition = ConvertTime(_player.CurrentTime);

                if (Math.Abs((controlPosition - Element.Position).TotalSeconds) > 1)
                    _player.Seek(CMTime.FromSeconds(Element.Position.TotalSeconds, 1));
            }
            else if (args.PropertyName == nameof(Element.Aspect))
            {
                _playerViewController.VideoGravity = (AVLayerVideoGravity) Element.Aspect;
            }
        }

        private void SetAreTransportControlsEnabled()
        {
            ((AVPlayerViewController) ViewController).ShowsPlaybackControls = Element.NativeControls;
        }

        private void SetSource()
        {
            AVAsset asset = null;

            if (Element.Source is UriVideoSource)
            {
                var uri = (Element.Source as UriVideoSource).Uri;

                if (!string.IsNullOrWhiteSpace(uri)) asset = AVAsset.FromUrl(new NSUrl(uri));
            }
            else if (Element.Source is FileVideoSource)
            {
                var uri = (Element.Source as FileVideoSource).File;

                if (!string.IsNullOrWhiteSpace(uri)) asset = AVAsset.FromUrl(new NSUrl(uri));
            }
            else if (Element.Source is ResourceVideoSource)
            {
                var path = (Element.Source as ResourceVideoSource).Path;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    var directory = Path.GetDirectoryName(path);
                    var filename = Path.GetFileNameWithoutExtension(path);
                    var extension = Path.GetExtension(path).Substring(1);
                    var url = NSBundle.MainBundle.GetUrlForResource(filename, extension, directory);
                    asset = AVAsset.FromUrl(url);
                }
            }

            if (asset != null)
            {
                _playerItem = new AVPlayerItem(asset);
                _videoEndNotificationToken =
                    NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification,
                        VideoDidFinishPlaying, _playerItem);
            }
            else
            {
                _playerItem = null;
            }

            _player.ReplaceCurrentItemWithPlayerItem(_playerItem);

            if (_playerItem != null && Element.AutoPlay) _player.Play();
        }

        // Event handler to update status
        private void OnUpdateStatus(object sender, EventArgs args)
        {
            var videoStatus = VideoStatus.NotReady;

            switch (_player.Status)
            {
                case AVPlayerStatus.ReadyToPlay:
                    switch (_player.TimeControlStatus)
                    {
                        case AVPlayerTimeControlStatus.Playing:
                            videoStatus = VideoStatus.Playing;
                            break;

                        case AVPlayerTimeControlStatus.Paused:
                            videoStatus = VideoStatus.Paused;
                            break;
                    }

                    break;
            }

            ((IVideoPlayerController) Element).Status = videoStatus;

            if (_playerItem != null)
            {
                ((IVideoPlayerController) Element).Duration = ConvertTime(_playerItem.Duration);
                ((IElementController) Element).SetValueFromRenderer(VideoPlayer.PositionProperty,
                    ConvertTime(_playerItem.CurrentTime));
            }
        }

        private TimeSpan ConvertTime(CMTime cmTime)
        {
            return TimeSpan.FromSeconds(double.IsNaN(cmTime.Seconds) ? 0 : cmTime.Seconds);
        }

        private void VideoDidFinishPlaying(NSNotification obj)
        {
            if (Element.Loop)
            {
                _player.Seek(new CMTime(0, 1));
                _player.Play();
            }
        }

        // Event handlers to implement methods
        private void OnPlayRequested(object sender, EventArgs args)
        {
            _player.Play();
        }

        private void OnPauseRequested(object sender, EventArgs args)
        {
            _player.Pause();
        }

        private void OnStopRequested(object sender, EventArgs args)
        {
            _player.Pause();
            _player.Seek(new CMTime(0, 1));
        }
    }
}