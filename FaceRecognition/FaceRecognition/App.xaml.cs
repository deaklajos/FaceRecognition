﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FaceRecognition.Views;
using PCLAppConfig;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FaceRecognition
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
