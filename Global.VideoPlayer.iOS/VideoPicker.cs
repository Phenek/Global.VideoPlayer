using System;
using System.Threading.Tasks;
using Global.VideoPlayer.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(VideoPicker))]

namespace Global.VideoPlayer.iOS
{
    public class VideoPicker : IVideoPicker
    {
        private UIImagePickerController _imagePicker;
        private TaskCompletionSource<string> _taskCompletionSource;

        public Task<string> GetVideoFileAsync()
        {
            // Create and define UIImagePickerController
            _imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.SavedPhotosAlbum,
                MediaTypes = new[] {"public.movie"}
            };

            // Set event handlers
            _imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;
            _imagePicker.Canceled += OnImagePickerCancelled;

            // Present UIImagePickerController;
            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.PresentModalViewController(_imagePicker, true);

            // Return Task object
            _taskCompletionSource = new TaskCompletionSource<string>();
            return _taskCompletionSource.Task;
        }

        private void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            if (args.MediaType == "public.movie")
                _taskCompletionSource.SetResult(args.MediaUrl.AbsoluteString);
            else
                _taskCompletionSource.SetResult(null);
            _imagePicker.DismissModalViewController(true);
            DetachHandlers();
        }

        private void OnImagePickerCancelled(object sender, EventArgs args)
        {
            _taskCompletionSource.SetResult(null);
            _imagePicker.DismissModalViewController(true);
            DetachHandlers();
        }

        private void DetachHandlers()
        {
            _imagePicker.FinishedPickingMedia -= OnImagePickerFinishedPickingMedia;
            _imagePicker.Canceled -= OnImagePickerCancelled;
        }
    }
}