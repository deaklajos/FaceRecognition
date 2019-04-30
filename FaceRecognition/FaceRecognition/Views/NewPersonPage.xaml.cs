using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FaceRecognition.Models;
using FaceRecognition.ViewModels;

namespace FaceRecognition.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPersonPage : ContentPage
    {
        ImageViewModel viewModel;

        public NewPersonPage(ImageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public NewPersonPage()
        {
            InitializeComponent();

            var item = new Person
            {
                name = "Name"
            };

            viewModel = new ImageViewModel(item);
            BindingContext = viewModel;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if(!viewModel.IsImageSet)
            {
                await DisplayAlert("Add an image!", "Before saving the person you must add an image.", "OK");
                return;
            }
                
            MessagingCenter.Send(this, "AddItem", viewModel.Person);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Camera_Clicked(object sender, EventArgs e)
        {
            await viewModel.TakePhotoAsync(this);
        }

        async void Pick_Clicked(object sender, EventArgs e)
        {
            await viewModel.PickImageAsync(this);
        }
    }
}