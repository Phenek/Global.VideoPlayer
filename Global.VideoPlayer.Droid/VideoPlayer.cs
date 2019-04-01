using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace Global.VideoPlayer.Droid
{
    public class VideoPlayer
    {
        public static readonly int PickImageId = 1000;

        // Field, properties, and method for Video Picker
        public static Activity Current { private set; get; }
        public static TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }

        public static void Init(Activity activity, Bundle bundle)
        {
            Current = activity;
        }

        public static void OnVideoPickerResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode != PickImageId) return;
            
            if (resultCode == Result.Ok && data != null)
                PickImageTaskCompletionSource.SetResult(data.DataString);
            else
                PickImageTaskCompletionSource.SetResult(null);
        }
    }
}