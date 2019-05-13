using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace FaceRecognition.ViewModels
{
    /// <summary>
    /// ViewModel for AboutPage
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/deaklajos/FaceRecognition")));
        }

        /// <summary>
        /// Command that opens a webpage.
        /// </summary>
        public ICommand OpenWebCommand { get; }
    }
}