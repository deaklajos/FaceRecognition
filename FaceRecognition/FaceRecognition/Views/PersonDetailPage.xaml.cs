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

        public PersonDetailPage(PersonDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
        
        public PersonDetailPage()
        {
            InitializeComponent();

            var item = new Person
            {
                Name = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new PersonDetailViewModel(item);
            BindingContext = viewModel;
        }

        async void Indentify_Clicked(object sender, EventArgs e)
        {
            // TODO implement
            await Navigation.PopAsync();
        }
    }
}