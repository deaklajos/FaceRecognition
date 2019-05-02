using FaceRecognition.Models;
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

        public RecognitionPage(RecognitionViewModel viewModel, Person person = null)
        {
            InitializeComponent();

            viewModel.Person = person;
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Person = person;
        }

        public RecognitionPage(Person person = null)
        {
            InitializeComponent();

            
            viewModel = new RecognitionViewModel();
            BindingContext = viewModel;
            viewModel.Person = person;
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

            try
            {
                var image = await viewModel.GetImageStreamAsync();
                var data = await viewModel.IndentifyAsync();
                await DisplayAlert("Done!", "Found faces: " + data.Count, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }
    }
}