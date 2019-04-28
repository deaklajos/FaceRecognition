using FaceRecognition.Services;
using System;
using Xamarin.Forms;

namespace FaceRecognition
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            CameraButton.Clicked += CameraButton_Clicked;
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            var provider = new ImageProvider();
            PhotoImage.Source = await provider.PickImageAsync(this);
        }
    }
}
