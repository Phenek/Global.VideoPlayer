using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Global.VideoPlayer.Sample.Droid
{
    [Activity(Label = "Global.VideoPlayer.Sample", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Forms.Init(this, bundle);

            Global.VideoPlayer.Droid.VideoPlayer.Init(this, bundle);

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Global.VideoPlayer.Droid.VideoPlayer.OnVideoPickerResult(requestCode, resultCode, data);
        }
    }
}