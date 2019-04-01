using System;
using Xamarin.Forms;

namespace Global.VideoPlayer
{
    public class PositionSlider : Slider
    {
        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create(nameof(Duration), typeof(TimeSpan), typeof(PositionSlider), new TimeSpan(1),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var seconds = ((TimeSpan) newValue).TotalSeconds;
                    ((Slider) bindable).Maximum = seconds <= 0 ? 1 : seconds;
                });

        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(TimeSpan), typeof(PositionSlider), new TimeSpan(0),
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var seconds = ((TimeSpan) newValue).TotalSeconds;
                    ((Slider) bindable).Value = seconds;
                });

        public PositionSlider()
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != "Value") return;
                var newPosition = TimeSpan.FromSeconds(Value);

                if (Math.Abs(newPosition.TotalSeconds - Position.TotalSeconds) / Duration.TotalSeconds > 0.01)
                    Position = newPosition;
            };
        }

        public TimeSpan Duration
        {
            set => SetValue(DurationProperty, value);
            get => (TimeSpan) GetValue(DurationProperty);
        }

        public TimeSpan Position
        {
            set => SetValue(PositionProperty, value);
            get => (TimeSpan) GetValue(PositionProperty);
        }
    }
}