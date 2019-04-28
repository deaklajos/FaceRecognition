using FaceRecognition.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceRecognition.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionPage : ContentPage
    {
        RecognitionViewModel viewModel;

        public RecognitionPage(RecognitionViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public RecognitionPage()
        {
            InitializeComponent();

            viewModel = new RecognitionViewModel();
            BindingContext = viewModel;
        }

        async void Camera_Clicked(object sender, EventArgs e)
        {
            var image = await viewModel.ImageProvider.TakePhotoAsync(this);
            if (image != null)
            {
                viewModel.Image = image;
            }
        }

        async void Picked_Clicked(object sender, EventArgs e)
        {
            var image = await viewModel.ImageProvider.PickImageAsync(this);
            if (image != null)
            {
                viewModel.Image = image;
            }
        }
    }
}