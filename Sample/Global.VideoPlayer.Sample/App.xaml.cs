using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Global.VideoPlayer.Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public class StartPage : ContentPage
    {
        public StartPage()
        {
            var toBindToVideoPlayerPageBtn = new Button {Text = "Bind To Video Player Page"};
            toBindToVideoPlayerPageBtn.Clicked += (sender, e) => { Navigation.PushAsync(new BindToVideoPlayerPage()); };

            var toCustomPositionBarPageBtn = new Button {Text = "Custom Position Bar Page"};
            toCustomPositionBarPageBtn.Clicked += (sender, e) => { Navigation.PushAsync(new CustomPositionBarPage()); };

            var toCustomTransportPageBtn = new Button {Text = "Custom Transport Page"};
            toCustomTransportPageBtn.Clicked += (sender, e) => { Navigation.PushAsync(new CustomTransportPage()); };

            var toPlayLibraryVideoPageBtn = new Button {Text = "Play Library Video Page"};
            toPlayLibraryVideoPageBtn.Clicked += (sender, e) => { Navigation.PushAsync(new PlayLibraryVideoPage()); };

            var toPlayVideoResourcePageBtn = new Button {Text = "Play Video Resource Page"};
            toPlayVideoResourcePageBtn.Clicked += (sender, e) => { Navigation.PushAsync(new PlayVideoResourcePage()); };

            var toPlayWebVideoPageBtn = new Button {Text = "Play Web Video Page"};
            toPlayWebVideoPageBtn.Clicked += (sender, e) => { Navigation.PushAsync(new PlayWebVideoPage()); };


            var toSelectWebVideoPageBtn = new Button {Text = "Select Web Video Page"};
            toSelectWebVideoPageBtn.Clicked += (sender, e) => { Navigation.PushAsync(new SelectWebVideoPage()); };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        toBindToVideoPlayerPageBtn,
                        toCustomPositionBarPageBtn,
                        toCustomTransportPageBtn,
                        toPlayLibraryVideoPageBtn,
                        toPlayVideoResourcePageBtn,
                        toPlayWebVideoPageBtn,
                        toSelectWebVideoPageBtn
                    }
                }
            };
        }
    }
}