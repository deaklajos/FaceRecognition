using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FaceRecognition.Models;
using FaceRecognition.ViewModels;

namespace FaceRecognition.Views
{
    /// <summary>
    /// Page for displaying the list of people.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PeoplePage : ContentPage
    {
        PeopleViewModel viewModel;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PeoplePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new PeopleViewModel();

            // Display server connection error.
            // Mostly internet turned off.
            MessagingCenter.Subscribe<PeopleViewModel, string>(this, "LoadException", async (obj, message) =>
            {
                await DisplayAlert("Error!", message, "OK");
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Person;
            if (item == null)
                return;

            await Navigation.PushAsync(new PersonDetailPage(new PersonDetailViewModel(item), viewModel));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPersonPage(viewModel));
        }

        async void Train_Clicked(object sender, EventArgs e)
        {
            try
            {
                await viewModel.FaceAPIWrapper.TrainPersonGroup();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.People.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}