using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace Global.VideoPlayer.Droid
{
    public class Global
    {
        // Field, properties, and method for Video Picker
        public static Activity Current { private set; get; }
        public static TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }
        public static readonly int PickImageId = 1000;

        public static void Init(Activity activity, Bundle bundle)
        {
        }

        public void OnVideoPickerResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == PickImageId)
            {
                if (resultCode == Result.Ok && data != null)
                    PickImageTaskCompletionSource.SetResult(data.DataString);
                else
                    PickImageTaskCompletionSource.SetResult(null);
            }
        }
    }
}
