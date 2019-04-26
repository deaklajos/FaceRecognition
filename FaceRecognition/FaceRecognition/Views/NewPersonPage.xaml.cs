using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FaceRecognition.Models;

namespace FaceRecognition.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPersonPage : ContentPage
    {
        public Person Person { get; set; }

        public NewPersonPage()
        {
            InitializeComponent();

            Person = new Person
            {
                Name = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Person);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Camera_Clicked(object sender, EventArgs e)
        {
            // TODO implement
            await Navigation.PopModalAsync();
        }

        async void Picked_Clicked(object sender, EventArgs e)
        {
            // TODO implement
            await Navigation.PopModalAsync();
        }
    }
}