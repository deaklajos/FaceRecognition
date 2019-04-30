﻿using System;

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
                name = "Item 1"
            };

            viewModel = new PersonDetailViewModel(item);
            BindingContext = viewModel;
        }

        async void Recognize_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecognitionPage());
        }
    }
}