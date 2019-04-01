using Xamarin.Forms;

namespace Global.VideoPlayer.Sample
{
    public partial class SelectWebVideoPage : ContentPage
    {
        public SelectWebVideoPage()
        {
            InitializeComponent();
        }

        public void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem == null) return;
            
            var key = ((string)args.SelectedItem).Replace(" ", "").Replace("'", "");
            videoPlayer.Source = (UriVideoSource)Application.Current.Resources[key];
        }
    }
}