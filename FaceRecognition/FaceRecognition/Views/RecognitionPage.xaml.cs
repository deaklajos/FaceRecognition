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
            // TODO implement
            await Navigation.PopAsync();
        }

        async void Picked_Clicked(object sender, EventArgs e)
        {
            // TODO implement
            await Navigation.PopAsync();
        }
    }
}