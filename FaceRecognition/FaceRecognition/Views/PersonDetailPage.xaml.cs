using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FaceRecognition.Models;
using FaceRecognition.ViewModels;

namespace FaceRecognition.Views
{
    /// <summary>
    /// Page for displaying a person.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonDetailPage : ContentPage
    {
        PersonDetailViewModel viewModel;
        PeopleViewModel PeopleViewModel;

        /// <summary>
        /// Contreuctor.
        /// </summary>
        /// <param name="viewModel">ViewModel for the page.</param>
        /// <param name="peopleViewModel">PeopleViewModel for deleting the person.</param>
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
    }
}