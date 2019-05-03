using FaceRecognition.Models;
using FaceRecognition.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceRecognition.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionPage : ContentPage
    {
        RecognitionViewModel viewModel;
        SKBitmap bitmap;

        public RecognitionPage(RecognitionViewModel viewModel, Person person = null)
        {
            InitializeComponent();

            string resourceID = "FaceRecognition.placeholder_image.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                bitmap = SKBitmap.Decode(stream);
            }

            viewModel.Person = person;
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Person = person;
        }

        public RecognitionPage(Person person = null)
        {
            InitializeComponent();

            string resourceID = "FaceRecognition.placeholder_image.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                bitmap = SKBitmap.Decode(stream);
            }

            viewModel = new RecognitionViewModel();
            BindingContext = viewModel;
            viewModel.Person = person;
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

        async void Camera_Clicked(object sender, EventArgs e)
        {
            await viewModel.TakePhotoAsync(this);

            var imageStream = await viewModel.GetImageStreamAsync();
            bitmap = SKBitmap.Decode(imageStream);
            canvasView.InvalidateSurface();
        }

        async void Pick_Clicked(object sender, EventArgs e)
        {
            await viewModel.PickImageAsync(this);

            var imageStream = await viewModel.GetImageStreamAsync();
            bitmap = SKBitmap.Decode(imageStream);
            canvasView.InvalidateSurface();
        }

        async void Recognize_Clicked(object sender, EventArgs e)
        {
            if(!viewModel.IsImageSet)
            {
                await DisplayAlert("Add an image!", "Before recognition you must add an image.", "OK");
                return;
            }

            try
            {
                var image = await viewModel.GetImageStreamAsync();
                var data = await viewModel.IndentifyAsync();
                await DisplayAlert("Done!", "Found faces: " + data.Count, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }
    }
}