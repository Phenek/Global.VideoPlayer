using System;
using Xamarin.Forms;

namespace Global.VideoPlayer
{
    public class VideoPlayer : View, IVideoPlayerController
    {
        public static readonly BindableProperty AspectProperty =
            BindableProperty.Create(nameof(Aspect), typeof(VideoAspect), typeof(VideoPlayer), VideoAspect.AspectFit,
                BindingMode.TwoWay);

        public static readonly BindableProperty LoopProperty =
            BindableProperty.Create(nameof(Loop), typeof(bool), typeof(VideoPlayer), false, BindingMode.TwoWay);

        // AreTransportControlsEnabled property
        public static readonly BindableProperty NativeControlsProperty =
            BindableProperty.Create(nameof(NativeControls), typeof(bool), typeof(VideoPlayer), true);

        // Source property
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(VideoSource), typeof(VideoPlayer), null);

        // AutoPlay property
        public static readonly BindableProperty AutoPlayProperty =
            BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(VideoPlayer), true);

        // Status read-only property
        private static readonly BindablePropertyKey StatusPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Status), typeof(VideoStatus), typeof(VideoPlayer),
                VideoStatus.NotReady);

        public static readonly BindableProperty StatusProperty = StatusPropertyKey.BindableProperty;

        // Duration read-only property
        private static readonly BindablePropertyKey DurationPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Duration), typeof(TimeSpan), typeof(VideoPlayer), new TimeSpan(),
                propertyChanged: (bindable, oldValue, newValue) => ((VideoPlayer) bindable).SetTimeToEnd());

        public static readonly BindableProperty DurationProperty = DurationPropertyKey.BindableProperty;

        // Position property
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(TimeSpan), typeof(VideoPlayer), new TimeSpan(),
                propertyChanged: (bindable, oldValue, newValue) => ((VideoPlayer) bindable).SetTimeToEnd());

        // TimeToEnd property
        private static readonly BindablePropertyKey TimeToEndPropertyKey =
            BindableProperty.CreateReadOnly(nameof(TimeToEnd), typeof(TimeSpan), typeof(VideoPlayer), new TimeSpan());

        public static readonly BindableProperty TimeToEndProperty = TimeToEndPropertyKey.BindableProperty;

        public VideoPlayer()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                UpdateStatus?.Invoke(this, EventArgs.Empty);
                return true;
            });
        }

        public VideoAspect Aspect
        {
            get => (VideoAspect) GetValue(AspectProperty);
            set => SetValue(AspectProperty, value);
        }

        public bool Loop
        {
            get => (bool) GetValue(LoopProperty);
            set => SetValue(LoopProperty, value);
        }

        public bool NativeControls
        {
            set => SetValue(NativeControlsProperty, value);
            get => (bool) GetValue(NativeControlsProperty);
        }

        [TypeConverter(typeof(VideoSourceConverter))]
        public VideoSource Source
        {
            set => SetValue(SourceProperty, value);
            get => (VideoSource) GetValue(SourceProperty);
        }

        public bool AutoPlay
        {
            set => SetValue(AutoPlayProperty, value);
            get => (bool) GetValue(AutoPlayProperty);
        }

        public VideoStatus Status => (VideoStatus) GetValue(StatusProperty);

        public TimeSpan Duration => (TimeSpan) GetValue(DurationProperty);

        public TimeSpan Position
        {
            set => SetValue(PositionProperty, value);
            get => (TimeSpan) GetValue(PositionProperty);
        }

        public TimeSpan TimeToEnd
        {
            private set => SetValue(TimeToEndPropertyKey, value);
            get => (TimeSpan) GetValue(TimeToEndProperty);
        }

        VideoStatus IVideoPlayerController.Status
        {
            set => SetValue(StatusPropertyKey, value);
            get => Status;
        }

        TimeSpan IVideoPlayerController.Duration
        {
            set => SetValue(DurationPropertyKey, value);
            get => Duration;
        }

        public event EventHandler UpdateStatus;

        private void SetTimeToEnd()
        {
            TimeToEnd = Duration - Position;
        }

        // Methods handled by renderers
        public event EventHandler PlayRequested;

        public void Play()
        {
            PlayRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler PauseRequested;

        public void Pause()
        {
            PauseRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler StopRequested;

        public void Stop()
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}