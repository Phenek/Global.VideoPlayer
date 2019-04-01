using System.Threading.Tasks;
using Android.Content;
using FormsVideoLibrary.Droid;
using Global.VideoPlayer;

using Xamarin.Forms;
// Need application's MainActivity

[assembly: Dependency(typeof(VideoPicker))]

namespace FormsVideoLibrary.Droid
{
    public class VideoPicker : IVideoPicker
    {
        public Task<string> GetVideoFileAsync()
        {
            // Define the Intent for getting images
            var intent = new Intent();
            intent.SetType("video/*");
            intent.SetAction(Intent.ActionGetContent);

            // Get the MainActivity instance
            var activity = Global.VideoPlayer.Droid.Global.Current;

            // Start the picture-picker activity (resumes in MainActivity.cs)
            activity.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Video"), Global.VideoPlayer.Droid.Global.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            Global.VideoPlayer.Droid.Global.PickImageTaskCompletionSource = new TaskCompletionSource<string>();

            // Return Task object
            return Global.VideoPlayer.Droid.Global.PickImageTaskCompletionSource.Task;
        }
    }
}