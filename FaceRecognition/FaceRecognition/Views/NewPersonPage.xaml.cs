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
        PeopleViewModel PeopleViewModel;

        public NewPersonPage(PeopleViewModel peopleViewModel)
        {
            InitializeComponent();

            var item = new Person
            {
                name = "Name"
            };

            viewModel = new ImageViewModel(item);
            BindingContext = viewModel;
            PeopleViewModel = peopleViewModel;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if(!viewModel.IsImageSet)
            {
                await DisplayAlert("Add an image!", "Before saving the person you must add an image.", "OK");
                return;
            }

            try
            {
                var imageStream = await viewModel.GetImageStreamAsync();
                await PeopleViewModel.AddPersonAsync(viewModel.Person, imageStream);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
                return;
            }

            await Navigation.PopAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
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