using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FaceRecognition.Models;
using FaceRecognition.ViewModels;
using SkiaSharp;
using System.Threading.Tasks;
using SkiaSharp.Views.Forms;
using System.Reflection;
using System.IO;
using Acr.UserDialogs;

namespace FaceRecognition.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPersonPage : ContentPage
    {
        ImageViewModel viewModel;
        PeopleViewModel PeopleViewModel;

        SKBitmap bitmap;

        public NewPersonPage(PeopleViewModel peopleViewModel)
        {
            InitializeComponent();

            string resourceID = "FaceRecognition.placeholder_image.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                bitmap = SKBitmap.Decode(stream);
            }

            var item = new Person
            {
                name = "Name"
            };

            viewModel = new ImageViewModel(item);
            BindingContext = viewModel;
            PeopleViewModel = peopleViewModel;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            float scale = Math.Min((float)info.Width / bitmap.Width,
                                   (float)info.Height / bitmap.Height);
            float x = (info.Width - scale * bitmap.Width) / 2;
            float y = (info.Height - scale * bitmap.Height) / 2;
            SKRect destRect = new SKRect(x, y, x + scale * bitmap.Width,
                                               y + scale * bitmap.Height);

            canvas.DrawBitmap(bitmap, destRect);
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (!viewModel.IsImageSet)
            {
                await DisplayAlert("Add an image!", "Before saving the person you must add an image.", "OK");
                return;
            }

            if (viewModel.Person.name.Length == 0)
            {
                await DisplayAlert("Invalid Name!", "Name can not be empty.", "OK");
                return;
            }

            try
            {
                using (UserDialogs.Instance.Loading("Processing", null, null, true, MaskType.Black))
                {
                    var imageStream = await viewModel.GetImageStreamAsync();
                    await PeopleViewModel.AddPersonAsync(viewModel.Person, imageStream);
                }  
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
            if (!viewModel.IsImageSet) return;

            var imageStream = await viewModel.GetImageStreamAsync();
            bitmap = SKBitmap.Decode(imageStream);
            canvasView.InvalidateSurface();
        }

        async void Pick_Clicked(object sender, EventArgs e)
        {
            await viewModel.PickImageAsync(this);
            if (!viewModel.IsImageSet) return;

            var imageStream = await viewModel.GetImageStreamAsync();
            bitmap = SKBitmap.Decode(imageStream);
            canvasView.InvalidateSurface();
        }
    }
}