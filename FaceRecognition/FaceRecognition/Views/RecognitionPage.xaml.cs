﻿using Acr.UserDialogs;
using FaceRecognition.Models;
using FaceRecognition.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FaceRecognition.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionPage : ContentPage
    {
        RecognitionViewModel viewModel;
        SKBitmap bitmap;
        IEnumerable<RectangleData> RectangleDatas = new List<RectangleData>();

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

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                IsAntialias = true,
                StrokeWidth = 2
            };

            SKPaint paintText = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = Color.Red.ToSKColor(),
                IsAntialias = true,
                StrokeWidth = 1,
                TextSize = 30
            };

            foreach (var item in RectangleDatas)
            {
                float rect_x = ((float)item.Facerectangle.left / bitmap.Width) * destRect.Width + destRect.Left;
                float rect_y = ((float)item.Facerectangle.top / bitmap.Height) * destRect.Height + destRect.Top;
                float rect_w = ((float)item.Facerectangle.width / bitmap.Width) * destRect.Width;
                float rect_h = ((float)item.Facerectangle.height / bitmap.Height) * destRect.Height;

                canvas.DrawRect(rect_x, rect_y, rect_w, rect_h, paint);

                var stringToDraw = $"{item.Name}, {item.Faceattributes.age}, {item.Faceattributes.gender}";
                canvas.DrawText(stringToDraw, rect_x - 20, rect_y - 20, paintText);
            }
        }

        async void Camera_Clicked(object sender, EventArgs e)
        {
            await viewModel.TakePhotoAsync(this);
            if (!viewModel.IsImageSetCompleted) return;

            // TODO check if changed, not set.
            var imageStream = await viewModel.GetImageStreamAsync();
            bitmap = SKBitmap.Decode(imageStream);
            RectangleDatas = new List<RectangleData>();

            canvasView.InvalidateSurface();
        }

        async void Pick_Clicked(object sender, EventArgs e)
        {
            await viewModel.PickImageAsync(this);
            if (!viewModel.IsImageSetCompleted) return;

            var imageStream = await viewModel.GetImageStreamAsync();
            bitmap = SKBitmap.Decode(imageStream);
            RectangleDatas = new List<RectangleData>();

            canvasView.InvalidateSurface();
        }

        async void Recognize_Clicked(object sender, EventArgs e)
        {
            if (!viewModel.IsImageSet)
            {
                await DisplayAlert("Add an image!", "Before recognition you must add an image.", "OK");
                return;
            }

            try
            {
                using (UserDialogs.Instance.Loading("Processing", null, null, true, MaskType.Black))
                {
                    var image = await viewModel.GetImageStreamAsync();
                    RectangleDatas = await viewModel.IdentifyAsync();
                    canvasView.InvalidateSurface();
                } 
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }  
        }
    }
}