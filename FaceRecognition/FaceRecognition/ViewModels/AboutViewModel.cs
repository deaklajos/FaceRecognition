using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace FaceRecognition.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/deaklajos/FaceRecognition")));
        }

        public ICommand OpenWebCommand { get; }
    }
}