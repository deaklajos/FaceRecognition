using FaceRecognition.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceRecognition.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionPage : ContentPage
    {
        ImageViewModel viewModel;

        public RecognitionPage(ImageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public RecognitionPage()
        {
            InitializeComponent();

            viewModel = new ImageViewModel();
            BindingContext = viewModel;
        }

        async void Camera_Clicked(object sender, EventArgs e)
        {
            await viewModel.TakePhotoAsync(this);
        }

        async void Pick_Clicked(object sender, EventArgs e)
        {
            await viewModel.PickImageAsync(this);
        }

        async void Recognize_Clicked(object sender, EventArgs e)
        {
            if(!viewModel.IsImageSet)
            {
                await DisplayAlert("Add an image!", "Before recognition you must add an image.", "OK");
                return;
            }

            //try
            //{
            //    var image = await viewModel.GetImageStreamAsync();
            //    var asd = await viewModel.FaceAPIWrapper.UploadAndDetectFaces(image);
            //    await DisplayAlert("Done!", "Found faces: " + asd.Count, "OK");
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("Error!", ex.Message, "OK");
            //}
        }
    }
}