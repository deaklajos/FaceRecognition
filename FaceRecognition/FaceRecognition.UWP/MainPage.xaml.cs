namespace FaceRecognition.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new FaceRecognition.App());
        }
    }
}
