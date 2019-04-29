using FaceRecognition.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace FaceRecognition.ViewModels
{
    public class RecognitionViewModel : BaseViewModel
    {
        public ImageProvider ImageProvider { get; } = new ImageProvider();
        public FaceAPIWrapper FaceAPIWrapper { get; } = new FaceAPIWrapper();

        private Stream imageStream;

        public async Task<Stream> GetImageStreamAsync()
        {
            // TODO test if only ImageSource requires copy
            // Copy required beause it seems like
            // ImageSource.FromStream calls Dispose
            // on the Stream.
            var ms = new MemoryStream();
            await imageStream.CopyToAsync(ms);
            // Reset the streams.
            imageStream.Position = 0;
            ms.Position = 0;
            return ms;
        }

        public async Task SetImageStreamAsync(Stream stream)
        {
            imageStream = stream;
            var tmpStream = await GetImageStreamAsync();
            Image = ImageSource.FromStream(() => { return tmpStream; });
        }

        //public Stream ImageStream
        //{
        //    get
        //    {
        //        // Copy required beause it seems like
        //        // ImageSource.FromStream calls Dispose
        //        // on the Stream.
        //        var ms = new MemoryStream();
        //        imageStream.CopyTo(ms); // TODO ASYNC COPY
        //        // Reset the streams.
        //        imageStream.Position = 0;
        //        ms.Position = 0;
        //        return ms;
        //    }
        //    set
        //    {
        //        imageStream = value;
        //        Image = ImageSource.FromStream(() => { return ImageStream; });
        //    }
        //}

        private ImageSource image = ImageSource.FromResource("FaceRecognition.baseline_photo.png");
        public ImageSource Image
        {
            get { return image; }
            private set
            {
                if (value != null)
                {
                    SetProperty(ref image, value);
                    IsImageSet = true;
                }
            }
        }

        public bool IsImageSet { get; private set; } = false;

        public RecognitionViewModel()
        {
            Title = "Recognition";
        }
    }
}