using FaceRecognition.Services;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace FaceRecognition.ViewModels
{
    public class RecognitionViewModel : BaseViewModel
    {
        public ImageProvider ImageProvider { get; } = new ImageProvider();

        private ImageSource image = ImageSource.FromResource("FaceRecognition.baseline_photo.png");
        public ImageSource Image
        {
            get { return image; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref image, value);
                    IsImageSet = true;
                }
            }
        }

        public bool IsImageSet { get; private set; } = false;

        // TODO rename.
        public RecognitionViewModel()
        {
            Title = "About";
        }
    }
}