using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FaceRecognition.Models;
using FaceRecognition.ViewModels;

namespace FaceRecognition.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonDetailPage : ContentPage
    {
        PersonDetailViewModel viewModel;
        PeopleViewModel PeopleViewModel;

        public PersonDetailPage(PersonDetailViewModel viewModel, PeopleViewModel peopleViewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            PeopleViewModel = peopleViewModel;
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            try
            {
                await PeopleViewModel.DeletePersonAsync(viewModel.Person);
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

        async void Recognize_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecognitionPage(viewModel.Person));
        }

        async void RecognizeAll_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecognitionPage());
        }
    }
}