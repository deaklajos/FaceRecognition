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
        NewPersonViewModel viewModel;

        public NewPersonPage(NewPersonViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public NewPersonPage()
        {
            InitializeComponent();

            // TODO add better or remove sample text.
            var item = new Person
            {
                Name = "Item name",
                Description = "This is an item description."
            };

            viewModel = new NewPersonViewModel(item);
            BindingContext = viewModel;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if(viewModel.Person.FaceImage == null)
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
            var image = await viewModel.imageProvider.TakePhotoAsync(this);
            if (image != null)
            {
                viewModel.Person.FaceImage = image;
                ImageDisplay.Source = viewModel.Person.FaceImage;
            }  
        }

        async void Pick_Clicked(object sender, EventArgs e)
        {
            var image = await viewModel.imageProvider.PickImageAsync(this);
            if (image != null)
            {
                viewModel.Person.FaceImage = image;
                ImageDisplay.Source = viewModel.Person.FaceImage;
            }
        }
    }
}